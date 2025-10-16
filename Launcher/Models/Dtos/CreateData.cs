using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Models.Dtos
{
    public class CreateData : JsonBase
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Lastupdate { get; set; } = string.Empty;
        public string ExeName { get; set; } = string.Empty;
        public string ImgName { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();

        public CreateData()
        {
            Type = "CreateData";
        }
    }
}
