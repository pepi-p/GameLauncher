using Launcher.Interfaces;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Launcher.Models;
using Launcher.Constants;

namespace Launcher.Services
{
    public class GameImageSaver : IGameImageSaver
    {
        private readonly IApiClient _api;
        private readonly IFileStreamSaver _fileStreamSaver;
        private readonly IDataRepository _dataRepository;
        private readonly IGameMetadataSaver _metadataSaver;

        public GameImageSaver(IApiClient api, IFileStreamSaver fileStreamSaver, IDataRepository dataRepository, IGameMetadataSaver metadataSaver, IWebSocketHandler webSocketHandler)
        {
            _api = api;
            _fileStreamSaver = fileStreamSaver;
            _dataRepository = dataRepository;
            _metadataSaver = metadataSaver;

            webSocketHandler.RegistrarAction("Image", (id) => Save(id));
        }

        public async Task<Result> Save(int id)
        {
            System.Diagnostics.Debug.WriteLine($"ImageSave: {id}");

            var dataResult = await _metadataSaver.Save(id);

            var data = dataResult.Value;
            var imgResult = await Save(data);
            if (!imgResult.IsSuccess) return Result.Failure(imgResult.ErrorMessage);

            _dataRepository.Registrar(id, data);

            return Result.Success();
        }

        public async Task<Result> Save(GameMetadata data)
        {
            System.Diagnostics.Debug.WriteLine($"ImageSave ImgName: {data.ImgName}");
            var dirPath = Path.Join(FilePaths.GamePath, data.DirName);

            var imageResult = await _api.GetGameImage(data.Id);
            if (!imageResult.IsSuccess) return Result.Failure(imageResult.ErrorMessage);

            var imgPath = Path.Join(dirPath, data.ImgName);
            var result = _fileStreamSaver.Save(imageResult.Value, imgPath);

            return result;
        }
    }
}
