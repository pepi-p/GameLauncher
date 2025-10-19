using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Models.Dtos;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Launcher.Services
{
    public class GameImageSaver : IGameImageSaver
    {
        private readonly IApiClient _api;
        private readonly IFileStreamSaver _fileStreamSaver;
        private readonly IDataRepository _dataRepository;
        private readonly IGameMetadataSaver _metadataSaver;
        private readonly ILogger _logger;

        public GameImageSaver(IApiClient api, IFileStreamSaver fileStreamSaver, IDataRepository dataRepository, IGameMetadataSaver metadataSaver, IWebSocketHandler webSocketHandler, ILogger logger)
        {
            _api = api;
            _fileStreamSaver = fileStreamSaver;
            _dataRepository = dataRepository;
            _metadataSaver = metadataSaver;
            _logger = logger;

            webSocketHandler.OnMessageReceived += async (object sender, MessageEventArgs e) => await Save(e);
        }

        public async Task<Result> Save(MessageEventArgs e)
        {
            if (e.Type == "ImgUpdateData")
            {
                _logger.Log(LogLevel.Info, $"[WS] 画像更新中 ({e.Type})");

                var data = JsonSerializer.Deserialize<ImgUpdateData>(e.Message);

                var dataResult = await _metadataSaver.SaveToRepository(data.Id);

                var meatadata = dataResult.Value;
                var imgResult = await Save(meatadata);
                if (!imgResult.IsSuccess)
                {
                    _logger.Log(LogLevel.Info, $"[WS] 画像更新失敗 ({imgResult.ErrorMessage})");
                    return Result.Failure(imgResult.ErrorMessage);
                }

                _dataRepository.Registrar(data.Id, meatadata);

                _logger.Log(LogLevel.Info, $"[WS] 画像更新成功 ({e.Type})");
                return Result.Success();
            }

            return Result.Failure("画像更新失敗. GameImageSaver.Save()");
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
