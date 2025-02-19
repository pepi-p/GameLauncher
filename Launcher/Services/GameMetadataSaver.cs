using Launcher.Interfaces;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Launcher.Constants;
using Launcher.Models;

namespace Launcher.Services
{
    public class GameMetadataSaver : IGameMetadataSaver
    {
        private readonly IApiClient _api;
        private readonly IDataRepository _dataRepository;

        public GameMetadataSaver(IApiClient api, IDataRepository dataRepository, IWebSocketHandler webSocketHandler)
        {
            _api = api;
            _dataRepository = dataRepository;

            webSocketHandler.RegistrarAction("Data", SaveToRepository);
        }

        public async Task<Result<GameMetadata>> Save(int id)
        {
            var dataResult = await _api.GetGameMetadata(id);
            if (!dataResult.IsSuccess) return Result<GameMetadata>.Failure(dataResult.ErrorMessage);

            var data = dataResult.Value;
            var dirPath = Path.Join(FilePaths.GamePath, data.DirName);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            var jsonSaveResult = JsonUtility.DumpFile(data, Path.Join(dirPath, "meta.json"));
            if (!jsonSaveResult.IsSuccess) return Result<GameMetadata>.Failure(jsonSaveResult.ErrorMessage);

            return Result<GameMetadata>.Success(data);
        }

        public async void SaveToRepository(int id)
        {
            var data = await Save(id);
            _dataRepository.Registrar(id, data.Value);
        }
    }
}
