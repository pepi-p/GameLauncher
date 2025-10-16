using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Services;
using Launcher.Views;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // フォルダ作成
            if (!Directory.Exists(FilePaths.RootPath)) Directory.CreateDirectory(FilePaths.RootPath);
            if (!Directory.Exists(FilePaths.ZipPath)) Directory.CreateDirectory(FilePaths.ZipPath);
            if (!Directory.Exists(FilePaths.GamePath)) Directory.CreateDirectory(FilePaths.GamePath);
            if (!Directory.Exists(FilePaths.LogPath)) Directory.CreateDirectory(FilePaths.LogPath);

            containerRegistry.RegisterForNavigation<Card>();
            containerRegistry.RegisterForNavigation<Detail>();
            containerRegistry.RegisterForNavigation<Tag>();

            var logger = new LoggerService($"{DateTime.Now.ToString("MMddHHmmss")}.txt");
            containerRegistry.RegisterInstance<ILogger>(logger);
            containerRegistry.RegisterInstance<ILoggerListener>(logger);
            var serverAddressProvider = new ServerAddressProvider(FilePaths.AddressTextPath);
            containerRegistry.RegisterInstance<IServerAddressProvider>(serverAddressProvider);
            containerRegistry.RegisterSingleton<IApiClient, ApiClient>();
            var webSocketClient = new WebSocketClient(serverAddressProvider);
            containerRegistry.RegisterInstance<IWebSocketHandler>(webSocketClient);
            containerRegistry.RegisterSingleton<IGameLoader, GameLoader>();
            containerRegistry.RegisterSingleton<IFileStreamSaver, FileStreamSaver>();
            containerRegistry.RegisterSingleton<IZipFileExtracter, ZipFileExtracter>();
            var dataRepository = new DataRepository();
            containerRegistry.RegisterInstance<IDataRepository>(dataRepository);
            containerRegistry.RegisterInstance<IDataRepositoryListener>(dataRepository);
            var detailRelay = new DetailDataRelay();
            containerRegistry.RegisterInstance<IDetail>(detailRelay);
            containerRegistry.RegisterInstance<IDetailListener>(detailRelay);
            containerRegistry.RegisterSingleton<IGameMetadataSaver, GameMetadataSaver>();
            containerRegistry.RegisterSingleton<IGameFileSaver, GameFileSaver>();
            containerRegistry.RegisterSingleton<IGameImageSaver, GameImageSaver>();
            containerRegistry.RegisterSingleton<IGameSaver, GameSaver>();

            var gameDeleter = new GameDeleter(dataRepository, webSocketClient);
            containerRegistry.RegisterInstance(gameDeleter);

            _ = webSocketClient.ConnectAsync();
        }
    }
}
