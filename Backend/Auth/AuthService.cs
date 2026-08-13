using Serilog;

namespace Segra.Backend.Auth
{
    /// <summary>
    /// Standalone/local-only auth shim.
    /// Remote Segra authentication is intentionally disabled for Fentware Clips standalone builds.
    /// </summary>
    public static class AuthService
    {
        public static void Login(string jwt, string refreshToken)
        {
            Log.Debug("Login ignored: standalone local-only mode");
        }

        public static void Logout()
        {
            // Clear any credentials that may have survived from an older Segra settings file.
            Core.Models.Settings.Instance.Auth.Jwt = string.Empty;
            Core.Models.Settings.Instance.Auth.RefreshToken = string.Empty;
            Log.Debug("Local auth state cleared in standalone mode");
        }

        public static bool IsAuthenticated() => false;

        public static Task<string> GetJwtAsync()
        {
            return Task.FromResult(string.Empty);
        }
    }
}
