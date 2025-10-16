using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Models.Dtos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Launcher.Services
{
    public class GameDeleter
    {
        private readonly IDataRepository _dataRepository;

        public GameDeleter(IDataRepository dataRepository, IWebSocketHandler webSocketHandler)
        {
            _dataRepository = dataRepository;

            webSocketHandler.OnMessageReceived += Delete;
        }

        public void Delete(object sender, MessageEventArgs e)
        {
            if (e.Type == "DeleteData")
            {
                var data = JsonSerializer.Deserialize<DeleteData>(e.Message);

                if (data is not null)
                {
                    var metadata = _dataRepository.GetData(data.Id);
                    var path = Path.Join(FilePaths.GamePath, metadata.DirName);

                    Directory.Delete(path, true);

                    _dataRepository.Unregistrar(data.Id); 
                }
            }
        }
    }
}
