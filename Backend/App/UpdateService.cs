using Serilog;
using Velopack;
using Velopack.Sources;

namespace Segra.Backend.App
{
    /// <summary>
    /// Standalone/local-only update service.
    ///
    /// The original Segra implementation queried upstream GitHub releases and downloaded
    /// Velopack updates. Fentware Clips standalone builds intentionally perform no remote
    /// update or release-note requests. The public API remains in place so the rest of the
    /// application can keep its existing local UI/state wiring without network access.
    /// </summary>
    public static class UpdateService
    {
        public static UpdateInfo? LatestUpdateInfo { get; private set; } = null;

        // Velopack metadata is still useful for determining the installed/current version.
        // These sources are never queried in standalone mode; all network-facing methods below
        // are disabled. Point them at the fork rather than the upstream Segra repository so no
        // upstream endpoint remains configured in this build.
        public static GithubSource Source = new("https://github.com/CoachSludge/Segra", null, false);
        public static GithubSource BetaSource = new("https://github.com/CoachSludge/Segra", null, true);
        public static UpdateManager UpdateManager { get; private set; } = new(Source);

        public static NuGet.Versioning.SemanticVersion GetCurrentVersion()
        {
            string? version = UpdateManager.CurrentVersion?.ToString()
                ?? System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString(3);

            if (!string.IsNullOrEmpty(version) &&
                NuGet.Versioning.SemanticVersion.TryParse(version, out var parsed))
            {
                return parsed;
            }

            Log.Warning("No Velopack or assembly version available; assuming a development build");
            return new NuGet.Versioning.SemanticVersion(9, 9, 9);
        }

        public static Task<bool> UpdateAppIfNecessary(bool forceCheck = false)
        {
            Log.Debug("Remote update check skipped: standalone local-only mode");
            return Task.FromResult(false);
        }

        public static void ApplyUpdate()
        {
            Log.Debug("ApplyUpdate ignored: standalone local-only mode");
        }

        public static Task SendCurrentUpdateProgressToFrontend()
        {
            return Task.CompletedTask;
        }

        public static Task<bool> ForceReinstallCurrentVersionAsync(CancellationToken ct = default)
        {
            Log.Debug("Force reinstall ignored: standalone local-only mode");
            return Task.FromResult(false);
        }

        public static async Task GetReleaseNotes(bool forceCheck = false)
        {
            // Preserve the frontend message contract, but never contact GitHub.
            await MessageService.SendFrontendMessage("ReleaseNotes", new
            {
                releaseNotesList = Array.Empty<object>()
            });
        }
    }
}
