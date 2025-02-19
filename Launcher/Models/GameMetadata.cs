using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Launcher.Models
{
    public struct GameMetadata
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public string Version { get; }
        public string LastUpdate { get; }
        public string Description { get; }
        public string ExeName { get; }
        public string ImgName { get; }
        public string DirName { get; }
        public List<string> Tags { get; }

        [JsonConstructor]
        public GameMetadata(int id, string title, string author, string version, string lastUpdate,
            string description, string exeName, string imgName, List<string> tags)
        {
            Id = id;
            Title = title;
            Author = author;
            Version = version;
            Description = description;
            LastUpdate = lastUpdate;
            ExeName = exeName;
            ImgName = imgName;
            DirName = $"{id}_{title}";
            Tags = tags;
        }
    }
}
