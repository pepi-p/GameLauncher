using Launcher.Interfaces;
using System;
using System.IO;

namespace Launcher.Services
{
    public class ServerAddressProvider : IServerAddressProvider
    {
        private readonly Uri _uri;

        public ServerAddressProvider(string filePath)
        {
            var address = ReadServerAddress(filePath);
            _uri = new Uri(address);
            System.Diagnostics.Debug.WriteLine(GetServerAddress());
        }

        private string ReadServerAddress(string filePath)
        {
            // ファイルが見つからない場合，ファイルを新規作成しlocalhostで初期化する
            if (!File.Exists(filePath)) File.WriteAllText(filePath, "http://127.0.0.1:8080/");

            return File.ReadAllText(filePath).Trim();
        }

        public string GetServerAddress()
        {
            return _uri.GetLeftPart(UriPartial.Authority);
        }

        public string GetWebSocketAddress()
        {
            return $"ws://{_uri.Host}:{_uri.Port}/ws/launcher";
        }
    }
}
