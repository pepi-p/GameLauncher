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
    public class GameMetadataSaver : IGameMetadataSaver
    {
        private readonly IApiClient _api;
        private readonly IDataRepository _dataRepository;
        private readonly ILogger _logger;

        public GameMetadataSaver(IApiClient api, IDataRepository dataRepository, IWebSocketHandler webSocketHandler, ILogger logger)
        {
            _api = api;
            _dataRepository = dataRepository;

            webSocketHandler.OnMessageReceived += (object sender, MessageEventArgs e) => Save(e);
            _logger = logger;
        }

        public Result<GameMetadata> Save(MessageEventArgs e)
        {
            if (e.Type == "UpdateData")
            {
                _logger.Log(LogLevel.Info, $"[WS] メタデータ更新中 ({e.Type})");

                var data = JsonSerializer.Deserialize<UpdateData>(e.Message);

                var metadata = Utilities.DotsConverter.UpdateDataToMetadata(data);
                var dirPath = Path.Join(FilePaths.GamePath, metadata.DirName);
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

                var jsonSaveResult = JsonUtility.DumpFile(metadata, Path.Join(dirPath, "meta.json"));
                if (!jsonSaveResult.IsSuccess) return Result<GameMetadata>.Failure(jsonSaveResult.ErrorMessage);

                _dataRepository.Registrar(metadata);

                _logger.Log(LogLevel.Info, "[WS] メタデータ更新成功");
                return Result<GameMetadata>.Success(metadata);
            }

            return Result<GameMetadata>.Failure("メタデータ保存失敗. GameMetadataSaver.Save()");
        }

        public async Task<Result<GameMetadata>> SaveToRepository(int id)
        {
            var dataResult = await _api.GetGameMetadata(id);
            if (!dataResult.IsSuccess) return Result<GameMetadata>.Failure(dataResult.ErrorMessage);

            var metadata = dataResult.Value;
            var dirPath = Path.Join(FilePaths.GamePath, metadata.DirName);
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);
            var jsonSaveResult = JsonUtility.DumpFile(metadata, Path.Join(dirPath, "meta.json"));
            if (!jsonSaveResult.IsSuccess) return Result<GameMetadata>.Failure(jsonSaveResult.ErrorMessage);

            _dataRepository.Registrar(id, metadata);

            return Result<GameMetadata>.Success(metadata);
        }
    }
}
