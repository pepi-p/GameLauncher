using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Interfaces
{
    public interface IServerAddressProvider
    {
        public string GetServerAddress();
        public string GetWebSocketAddress();
    }
}
