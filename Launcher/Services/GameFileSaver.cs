using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Launcher.Constants;

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

            webSocketHandler.RegistrarAction("File", (id) => Save(id));
        }

        public async Task<Result> Save(int id)
        {
            var data = _dataRepository.GetData(id);
            var fileResult = await Save(data);
            if (!fileResult.IsSuccess) return Result.Failure(fileResult.ErrorMessage);

            _metadataSaver.SaveToRepository(id);

            return Result.Success();
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
