using System;

namespace Launcher.Interfaces
{
    public interface ILoggerListener
    {
        /// <summary>
        /// ログ出力するための関数を登録する
        /// </summary>
        public event Action<string> OnLogReceived;
    }
}
