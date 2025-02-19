using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Launcher.Models
{
    public struct UpdateData
    {
        public int Id { get; }
        public string Command { get; }

        [JsonConstructor]
        public UpdateData(int id, string command)
        {
            Id = id;
            Command = command;
        }
    }
}
