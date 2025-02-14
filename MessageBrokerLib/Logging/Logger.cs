using System;
using System.IO;

namespace MessageBrokerLib.Logging
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error,
    }

    public static class Logger
    {
        private static readonly string LogFilePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "Storage", "log.txt"));
        public static LogLevel CurrentLogLevel { get; set; } = LogLevel.Info;

        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            if (level < CurrentLogLevel)
            {
                return;
            }

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] - {message}";
            try
            {
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                Console.WriteLine(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging Error: {ex.Message}");
            }
        }
    }
}
