using Launcher.Utilities;

namespace Launcher.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// ログを表示する
        /// </summary>
        /// <param name="logLevel">レベル</param>
        /// <param name="message">メッセージ</param>
        public void Log(LogLevel logLevel, string message);
    }
}
