using Launcher.Models;
using Launcher.Utilities;

namespace Launcher.Interfaces
{
    public interface IZipFileExtracter
    {
        /// <summary>
        /// zipファイルを解凍する
        /// </summary>
        /// <param name="sourcePath">参照元ファイル</param>
        /// <param name="destPath">展開先ディレクトリ</param>
        public Result FileUnZip(string sourcePath, string destPath);
    }
}
