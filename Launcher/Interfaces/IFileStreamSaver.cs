using Launcher.Models;
using Launcher.Utilities;
using System.IO;
using System.Threading.Tasks;

namespace Launcher.Interfaces
{
    public interface IFileStreamSaver
    {
        /// <summary>
        /// streamをファイルとして保存する
        /// </summary>
        /// <param name="stream">ファイルのstream</param>
        /// <param name="path">保存先パス</param>
        /// <returns></returns>
        public Result Save(Stream stream, string path);
    }
}
