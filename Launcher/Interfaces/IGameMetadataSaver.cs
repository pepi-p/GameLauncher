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
        public Result<GameMetadata> Save(MessageEventArgs e);
        public Task<Result<GameMetadata>> SaveToRepository(int id);
    }
}
