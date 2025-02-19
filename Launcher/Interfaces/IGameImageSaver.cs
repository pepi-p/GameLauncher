using Launcher.Models;
using Launcher.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher.Interfaces
{
    public interface IGameImageSaver
    {
        /// <summary>
        /// 画像ファイルをサーバから落とし，ローカルへ保存
        /// </summary>
        /// <param name="data">Metadata</param>
        /// <returns></returns>
        public Task<Result> Save(GameMetadata data);
    }
}
