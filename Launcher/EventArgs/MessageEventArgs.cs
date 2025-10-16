using System;

namespace Launcher
{
    public class MessageEventArgs : EventArgs
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public MessageEventArgs(string type, string msg)
        {
            Type = type;
            Message = msg;
        }
    }
}
