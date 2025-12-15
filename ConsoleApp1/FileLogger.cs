using System;
using System.IO;

namespace GameLibraryManager
{
    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;

        public FileLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            var line = $"{DateTime.Now:u} - {message}";
            File.AppendAllLines(_logFilePath, new[] { line });
        }
    }
}



