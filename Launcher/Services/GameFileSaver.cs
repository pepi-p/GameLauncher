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

        public GameFileSaver(IApiClient api, IFileStreamSaver fileStreamSaver, IZipFileExtracter zipFileExtracter, IDataRepository dataRepository, IGameMetadataSaver metadataSaver, IWebSocketHandler webSocketHandler)
        {
            _api = api;
            _fileStreamSaver = fileStreamSaver;
            _zipFileExtracter = zipFileExtracter;
            _dataRepository = dataRepository;
            _metadataSaver = metadataSaver;

            webSocketHandler.OnMessageReceived += async (object sender, MessageEventArgs e) => await Save(e);
        }

        public async Task<Result> Save(MessageEventArgs e)
        {
            if (e.Type == "ExeUpdateData")
            {
                var data = JsonSerializer.Deserialize<ExeUpdateData>(e.Message);

                if (data is not null)
                {
                    var metadata = _dataRepository.GetData(data.Id);
                    var fileResult = await Save(metadata);
                    if (!fileResult.IsSuccess) return Result.Failure(fileResult.ErrorMessage);

                    await _metadataSaver.SaveToRepository(data.Id);

                    return Result.Success();
                }
            }

            return Result.Failure("exe更新失敗. GameFileSaver.Save()");
        }

        public async Task<Result> Save(GameMetadata data)
        {
            var dirPath = Path.Join(FilePaths.GamePath, data.DirName);

            // ビルドファイルをダウンロード
            var zipPath = Path.Join(dirPath, $"{data.DirName}.zip");
            var zipResult = await _api.GetGameZip(data.Id);
            if (!zipResult.IsSuccess) return Result.Failure(zipResult.ErrorMessage);
            _fileStreamSaver.Save(zipResult.Value, zipPath);

            // 解凍
            var unzipDirPath = Path.Join(dirPath, data.Title);
            var unzipResult = _zipFileExtracter.FileUnZip(zipPath, unzipDirPath);

            return unzipResult;
        }
    }
}
