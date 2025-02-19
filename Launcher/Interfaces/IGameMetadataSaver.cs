using Launcher.Models;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Interfaces
{
    public interface IGameMetadataSaver
    {
        public Task<Result<GameMetadata>> Save(int id);
        public void SaveToRepository(int id);
    }
}
