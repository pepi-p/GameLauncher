using Launcher.Models;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Interfaces
{
    public interface IGameSaver
    {
        public Task<Result> Save(int id);
        public Task<Result> Save(GameMetadata data);
    }
}
