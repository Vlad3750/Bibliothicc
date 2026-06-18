using System;
using System.IO;

namespace Bibliothicc.Services
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Bibliothicc", "app.log");

        static Logger()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
        }

        public static void Info(string message) => Write("INFO", message);
        public static void Warn(string message) => Write("WARN", message);
        public static void Error(string message, Exception? ex = null)
        {
            Write("ERROR", ex == null ? message : $"{message} — {ex.Message}");
        }

        private static void Write(string level, string message)
        {
            try
            {
                File.AppendAllText(LogPath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}");
            }
            catch { }
        }
    }
}
