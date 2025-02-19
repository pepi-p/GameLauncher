using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Launcher.Services
{
    public class GameSaver : IGameSaver
    {
        private readonly IGameMetadataSaver _metadataSaver;
        private readonly IGameFileSaver _fileSaver;
        private readonly IGameImageSaver _imageSaver;
        private readonly IDataRepository _dataRepository;

        public GameSaver(IGameMetadataSaver metadataSaver, IGameFileSaver fileSaver, IGameImageSaver imageSaver, IDataRepository dataRepository, IWebSocketHandler webSocketHandler)
        {
            _metadataSaver = metadataSaver;
            _fileSaver = fileSaver;
            _imageSaver = imageSaver;
            _dataRepository = dataRepository;

            webSocketHandler.RegistrarAction("Create", (id) => Save(id));
        }

        public async Task<Result> Save(int id)
        {
            var metadataSave = await _metadataSaver.Save(id);
            if (!metadataSave.IsSuccess) return Result.Failure(metadataSave.ErrorMessage);

            var data = metadataSave.Value;

            var fileSaver = await _fileSaver.Save(data);
            if (!fileSaver.IsSuccess) return Result.Failure(fileSaver.ErrorMessage);

            var imageSaver = await _imageSaver.Save(data);
            if (!imageSaver.IsSuccess) return Result.Failure(imageSaver.ErrorMessage);

            _dataRepository.Registrar(id, data);

            return Result.Success();
        }

        public async Task<Result> Save(GameMetadata data)
        {
            var dirPath = Path.Join(FilePaths.GamePath, data.DirName);
            var jsonSaveResult = JsonUtility.DumpFile(data, Path.Join(dirPath, "meta.json"));
            if (!jsonSaveResult.IsSuccess) return Result.Failure(jsonSaveResult.ErrorMessage);

            var fileSaver = await _fileSaver.Save(data);
            if (!fileSaver.IsSuccess) return Result.Failure(fileSaver.ErrorMessage);

            var imageSaver = await _imageSaver.Save(data);
            if (!imageSaver.IsSuccess) return Result.Failure(imageSaver.ErrorMessage);

            _dataRepository.Registrar(data.Id, data);

            return Result.Success();
        }
    }
}
