using Launcher.Constants;
using Launcher.Models;
using Launcher.Utilities;
using System.Collections.Generic;
using System.IO;

namespace Launcher.Services
{
    /// <summary>
    /// ローカルに保存されているjsonからMetadataを読み込む
    /// </summary>
    public class GameMetadataLoader
    {
        public static Result<GameMetadata> Load(string dirName)
        {
            var jsonPath = Path.Join(FilePaths.GamePath, dirName, "meta.json");
            if (!File.Exists(jsonPath)) return Result<GameMetadata>.Failure($"jsonがありません: {dirName}");

            var data = JsonUtility.Deserialize<GameMetadata>(jsonPath);

            return data;
        }

        public static Result<List<GameMetadata>> LoadAll()
        {
            var datas = new List<GameMetadata>();
            var gameDirectorys = Directory.EnumerateDirectories(FilePaths.GamePath, "*", SearchOption.TopDirectoryOnly);

            foreach (var directory in gameDirectorys)
            {
                var dirName = Path.GetFileName(directory);
                var result = Load(dirName);
                if (result.IsSuccess) datas.Add(result.Value);
                else Result<List<GameMetadata>>.Failure(result.ErrorMessage);
            }

            return Result<List<GameMetadata>>.Success(datas);
        }
    }
}
