using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Interfaces
{
    public interface IWebSocketHandler
    {
        public void RegistrarAction(string key, Action<int> method);
    }
}
