using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Services;
using Launcher.Utilities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Launcher.Models
{
    /// <summary>
    /// ゲームの保存，解凍を行う
    /// </summary>
    public class GameLoader : IGameLoader
    {
        private readonly IApiClient _api;
        private readonly ILogger _logger;
        private readonly IDataRepository _dataRepository;
        private readonly IGameSaver _gameSaver;

        public GameLoader(IApiClient api, ILogger logger, IDataRepository dataRepository, IGameSaver gameSaver)
        {
            _api = api;
            _logger = logger;
            _dataRepository = dataRepository;
            _gameSaver = gameSaver;
        }

        public async Task UpdateAllGame()
        {
            var metadataResult = await _api.GetGameMetadataAll();
            if (!metadataResult.IsSuccess)
            {
                _logger.Log(LogLevel.Error, metadataResult.ErrorMessage);
                return;
            }

            foreach (var data in metadataResult.Value)
            {
                _logger.Log(LogLevel.Info, $"{data.Title}を更新中");

                // ディレクトリを確保する（存在しなければ新規作成）
                var dirPath = Path.Join(FilePaths.GamePath, data.DirName);
                if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

                var result = await _gameSaver.Save(data);
                if (!result.IsSuccess) _logger.Log(LogLevel.Error, result.ErrorMessage);
            }

            _logger.Log(LogLevel.Info, "更新終了");
        }
    }
}
