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
    public class GameSaver : IGameSaver
    {
        private readonly IGameMetadataSaver _metadataSaver;
        private readonly IGameFileSaver _fileSaver;
        private readonly IGameImageSaver _imageSaver;
        private readonly IDataRepository _dataRepository;
        private readonly ILogger _logger;

        public GameSaver(IGameMetadataSaver metadataSaver, IGameFileSaver fileSaver, IGameImageSaver imageSaver, IDataRepository dataRepository, IWebSocketHandler webSocketHandler, ILogger logger)
        {
            _metadataSaver = metadataSaver;
            _fileSaver = fileSaver;
            _imageSaver = imageSaver;
            _dataRepository = dataRepository;
            _logger = logger;

            webSocketHandler.OnMessageReceived += async (object sender, MessageEventArgs e) => await Save(e);
        }

        public async Task Save(MessageEventArgs e)
        {
            _logger.Log(LogLevel.Info, $"[WS] 新規作成 ({e.Type})");

            if (e.Type == "CreateData")
            {
                var data = JsonSerializer.Deserialize<CreateData>(e.Message);

                var metadata = Utilities.DotsConverter.CreateDataToMetadata(data);

                var fileSaver = await _fileSaver.Save(metadata);
                if (!fileSaver.IsSuccess)
                {
                    _logger.Log(LogLevel.Error, $"[WS] 新規作成失敗 ({fileSaver.ErrorMessage})");
                    return;
                }

                var imageSaver = await _imageSaver.Save(metadata);
                if (!imageSaver.IsSuccess)
                {
                    _logger.Log(LogLevel.Error, $"[WS] 新規作成失敗 ({imageSaver.ErrorMessage})");
                    return;
                }

                var dirPath = Path.Join(FilePaths.GamePath, metadata.DirName);
                var jsonSaveResult = JsonUtility.DumpFile(metadata, Path.Join(dirPath, "meta.json"));
                if (!jsonSaveResult.IsSuccess)
                {
                    _logger.Log(LogLevel.Error, $"[WS] 新規作成失敗 ({jsonSaveResult.ErrorMessage})");
                    return;
                }

                _dataRepository.Registrar(metadata.Id, metadata);
            }

            _logger.Log(LogLevel.Error, $"[WS] 新規作成失敗 ({e.Type})");
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
