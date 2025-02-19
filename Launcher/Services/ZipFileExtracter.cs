using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Utilities;
using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Shapes;

namespace Launcher.Services
{
    /// <summary>
    /// zipファイルを解凍するクラス
    /// </summary>
    public class ZipFileExtracter : IZipFileExtracter
    {
        private ILogger _logger;

        public ZipFileExtracter(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// zipファイルを解凍する
        /// </summary>
        /// <param name="sourcePath">参照元ファイル</param>
        /// <param name="destPath">展開先ディレクトリ</param>
        public Result FileUnZip(string sourcePath, string destPath)
        {
            System.Diagnostics.Debug.WriteLine("FileUnZip()");

            if (!File.Exists(sourcePath)) return Result.Failure("zipファイルが存在しません: " + sourcePath);

            try
            {
                // ディレクトリが既に存在していれば削除
                if (Directory.Exists(destPath))
                {
                    Directory.Delete(destPath, true);
                    _logger.Log(LogLevel.Info, $"重複のため削除: {destPath}");
                }

                ZipFile.ExtractToDirectory(sourcePath, destPath);

                return Result.Success();
            }
            catch (IOException e)
            {
                return Result.Failure($"ZipFileExtracter.FileUnZip(): {e.Message}");
            }
            catch (Exception e)
            {
                return Result.Failure($"ZipFileExtracter.FileUnZip(): {e.Message}");
            }
        }
    }
}
