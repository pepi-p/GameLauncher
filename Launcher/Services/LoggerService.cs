using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Utilities;
using System;
using System.IO;

namespace Launcher.Services
{
    public class LoggerService : ILogger, ILoggerListener
    {
        public event Action<string> OnLogReceived;
        private readonly string _logFileName;

        public LoggerService(string logFileName)
        {
            _logFileName = logFileName;
        }

        /// <summary>
        /// ログを表示する
        /// </summary>
        /// <param name="logLevel">レベル</param>
        /// <param name="message">メッセージ</param>
        public void Log(LogLevel logLevel, string message)
        {
            OnLogReceived?.Invoke(message);

            var logMessage = $"[{DateTime.Now.ToString("HH:mm:ss")}] [{logLevel}] {message}";
            WriteLogFile(logMessage);
            System.Diagnostics.Debug.WriteLine(logMessage);
        }

        private void WriteLogFile(string message)
        {
            var path = Path.Join(FilePaths.LogPath, _logFileName);
            File.AppendAllText(path, message + "\n");
        }
    }
}
