using Launcher.Models;
using System;

namespace Launcher.Interfaces
{
    public interface IDetailListener
    {
        public event Action<GameMetadata> OnDetailShow;
        public event Action OnDetailHide;
    }
}