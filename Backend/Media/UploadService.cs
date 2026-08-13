using Serilog;
using System.Text.Json;
using Segra.Backend.App;

namespace Segra.Backend.Media
{
    /// <summary>
    /// Standalone/local-only upload shim.
    /// Remote Segra clip uploads are intentionally disabled for Fentware Clips standalone builds.
    /// </summary>
    internal static class UploadService
    {
        public static void CancelUpload(string fileName)
        {
            Log.Debug("CancelUpload ignored for {FileName}: standalone local-only mode", fileName);
        }

        public static async Task HandleUploadContent(JsonElement message)
        {
            string title = message.TryGetProperty("Title", out var titleElement)
                ? titleElement.GetString() ?? "Clip"
                : "Clip";

            Log.Debug("UploadContent ignored: standalone local-only mode");

            await MessageService.SendFrontendMessage("UploadProgress", new
            {
                title,
                fileName = string.Empty,
                progress = 0,
                status = "error",
                message = "Online uploads are disabled in this standalone build."
            });
        }
    }
}
