using Launcher.Models;
using System;
using System.Collections.Generic;

namespace Launcher.Interfaces
{
    public interface IDataRepositoryListener
    {
        public event Action<Dictionary<int, GameMetadata>> OnDataChanged;
    }
}
