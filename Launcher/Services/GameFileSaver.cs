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
    public class GameFileSaver : IGameFileSaver
    {
        private readonly IApiClient _api;
        private readonly IFileStreamSaver _fileStreamSaver;
        private readonly IZipFileExtracter _zipFileExtracter;
        private readonly IDataRepository _dataRepository;
        private readonly IGameMetadataSaver _metadataSaver;
        private readonly ILogger _logger;

        public GameFileSaver(
            IApiClient api,
            IFileStreamSaver fileStreamSaver,
            IZipFileExtracter zipFileExtracter,
            IDataRepository dataRepository,
            IGameMetadataSaver metadataSaver,
            IWebSocketHandler webSocketHandler,
            ILogger logger
            )
        {
            _api = api;
            _fileStreamSaver = fileStreamSaver;
            _zipFileExtracter = zipFileExtracter;
            _dataRepository = dataRepository;
            _metadataSaver = metadataSaver;

            webSocketHandler.OnMessageReceived += async (object sender, MessageEventArgs e) => await Save(e);
            _logger = logger;
        }

        public async Task<Result> Save(MessageEventArgs e)
        {
            if (e.Type == "ExeUpdateData")
            {
                _logger.Log(LogLevel.Info, $"[WS] zip更新中 ({e.Type})");

                var data = JsonSerializer.Deserialize<ExeUpdateData>(e.Message);

                if (data is not null)
                {
                    var metadata = _dataRepository.GetData(data.Id);
                    var fileResult = await Save(metadata);
                    if (!fileResult.IsSuccess)
                    {
                        _logger.Log(LogLevel.Error, $"[WS] zip更新失敗 ({e.Type})");
                        return Result.Failure(fileResult.ErrorMessage);
                    }

                    var saveResult = await _metadataSaver.SaveToRepository(data.Id);
                    if (!saveResult.IsSuccess)
                    {
                        _logger.Log(LogLevel.Error, $"[WS] zip更新失敗 ({e.Type})");
                        return Result.Failure(saveResult.ErrorMessage);
                    }

                    _logger.Log(LogLevel.Info, $"[WS] zip更新完了 ({e.Type})");
                    return Result.Success();
                }
            }

            return Result.Failure("zip更新失敗. GameFileSaver.Save()");
        }

        public async Task<Result> Save(GameMetadata data)
        {
            var dirPath = Path.Join(FilePaths.GamePath, data.DirName);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            // ビルドファイルをダウンロード
            var zipPath = Path.Join(dirPath, $"{data.DirName}.zip");
            _logger.Log(LogLevel.Info, $"zipPath: {zipPath}");
            var zipResult = await _api.GetGameZip(data.Id);
            if (!zipResult.IsSuccess) return Result.Failure(zipResult.ErrorMessage);
            var fileStreamResult = _fileStreamSaver.Save(zipResult.Value, zipPath);
            if (!fileStreamResult.IsSuccess)
            {
                _logger.Log(LogLevel.Error, $"{fileStreamResult.ErrorMessage} (GameFileSaver.Save)");
            }

            // 解凍
            var unzipDirPath = Path.Join(dirPath, data.Title);
            var unzipResult = _zipFileExtracter.FileUnZip(zipPath, unzipDirPath);
            if (!unzipResult.IsSuccess)
            {
                _logger.Log(LogLevel.Error, $"{unzipResult.ErrorMessage} (GameFileSaver.Save)");
            }

            return unzipResult;
        }
    }
}
