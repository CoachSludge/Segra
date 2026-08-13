using Serilog;
using System.Net;
using System.Text;

namespace Segra.Backend.Auth
{
    /// <summary>
    /// Standalone/local-only Discord auth shim.
    /// Browser-based Segra authentication is intentionally unavailable in this build.
    /// </summary>
    internal static class DiscordLoginService
    {
        private const string CallbackPath = "/auth/callback";

        public static bool IsCallbackPath(string path) =>
            path.Equals(CallbackPath, StringComparison.OrdinalIgnoreCase);

        public static void Begin()
        {
            Log.Debug("Discord login ignored: standalone local-only mode");
        }

        public static void Cancel()
        {
            Log.Debug("Discord login cancel ignored: standalone local-only mode");
        }

        public static async Task HandleCallbackAsync(HttpListenerContext context)
        {
            const string html = """
                <!doctype html>
                <html lang="en">
                <head>
                  <meta charset="utf-8">
                  <meta name="viewport" content="width=device-width, initial-scale=1">
                  <title>Fentware Clips</title>
                </head>
                <body style="background:#050505;color:#eee;font-family:system-ui;padding:32px">
                  <h1>Fentware Clips</h1>
                  <p>Online sign-in is disabled in this standalone build.</p>
                </body>
                </html>
                """;

            byte[] buffer = Encoding.UTF8.GetBytes(html);
            context.Response.StatusCode = (int)HttpStatusCode.Gone;
            context.Response.ContentType = "text/html; charset=utf-8";
            context.Response.ContentLength64 = buffer.Length;
            await context.Response.OutputStream.WriteAsync(buffer);
        }
    }
}
