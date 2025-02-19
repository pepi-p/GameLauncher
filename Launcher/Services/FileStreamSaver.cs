using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Utilities;
using System.IO;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Launcher.Services
{
    /// <summary>
    /// サーバからzipファイルをダウンロードし，ローカルに保存する
    /// </summary>
    public class FileStreamSaver : IFileStreamSaver
    {
        private ILogger _logger;

        public FileStreamSaver(ILogger logger)
        {
            _logger = logger;
        }

        public Result Save(Stream stream, string path)
        {
            try
            {
                // 既に存在していれば削除
                if (File.Exists(path))
                {
                    File.Delete(path);
                    _logger.Log(LogLevel.Info, $"重複のため削除: {path}");
                }

                // streamをファイルとして保存
                using (var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    int read;
                    byte[] buffer = new byte[1048576];
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        file.Write(buffer, 0, read);
                    }
                }

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure($"FileStreamSaver.Save(): {e.Message}");
            }
        }
    }
}
