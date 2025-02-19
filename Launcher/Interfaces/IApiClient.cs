using Launcher.Models;
using Launcher.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Launcher.Interfaces
{
    public interface IApiClient
    {
        /// <summary>
        /// サーバのヘルスチェックを実行する
        /// サーバが稼働しているかどうかを確認するために使用する
        /// </summary>
        /// <returns>
        /// 成功した場合は true，失敗した場合は false を返す
        /// </returns>
        public Task<Result> HealthCheck();

        /// <summary>
        /// サーバからゲームのMetadataを取得する
        /// 指定したゲームのMetadataを取得するために使用する
        /// </summary>
        /// <returns>
        /// 成功した場合は true と，ゲームMetadataのリストを返す
        /// 失敗した場合は false と，エラーログを返す
        /// </returns>
        public Task<Result<GameMetadata>> GetGameMetadata(int id);

        /// <summary>
        /// サーバからゲームのMetadataリストを取得する
        /// すべてのゲームのMetadataを取得するために使用する
        /// </summary>
        /// <returns>
        /// 成功した場合は true と，ゲームMetadataのリストを返す
        /// 失敗した場合は false と，エラーログを返す
        /// </returns>
        public Task<Result<List<GameMetadata>>> GetGameMetadataAll();

        /// <summary>
        /// サーバから指定したゲームのzipファイルをダウンロードする
        /// ゲームのMetadataを基に，サーバからzipファイルを取得する
        /// </summary>
        /// <param name="id">ダウンロードするゲームのMetadata</param>
        /// <returns>
        /// 成功した場合は true と，ゲームのzipファイルストリームを返す
        /// 失敗した場合は false と，エラーログを返す
        /// </returns>
        public Task<Result<Stream>> GetGameZip(int id);

        /// <summary>
        /// サーバから指定したゲームのタイトル画像をダウンロードする
        /// ゲームのidを基に，サーバから画像ファイルを取得する
        /// </summary>
        /// <param name="id">ダウンロードするゲームのid</param>
        /// <returns>
        /// 成功した場合は true と，ゲームのzipファイルストリームを返す
        /// 失敗した場合は false と，エラーログを返す
        /// </returns>
        public Task<Result<Stream>> GetGameImage(int id);
    }
}
