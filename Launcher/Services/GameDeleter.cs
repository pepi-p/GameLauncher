using Launcher.Interfaces;
using Launcher.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Launcher.Services
{
    public class GameDeleter
    {
        private readonly IDataRepository _dataRepository;

        public GameDeleter(IDataRepository dataRepository, IWebSocketHandler webSocketHandler)
        {
            _dataRepository = dataRepository;

            webSocketHandler.RegistrarAction("Delete", Delete);
        }

        public async void Delete(int id)
        {
            var data = _dataRepository.GetData(id);
            var path = Path.Join(FilePaths.GamePath, data.DirName);

            Directory.Delete(path, true);

            _dataRepository.Unregistrar(id);
        }
    }
}
