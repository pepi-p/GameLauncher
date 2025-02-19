using Launcher.Constants;
using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Services;
using Launcher.Utilities;
using Launcher.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Launcher.ViewModels
{
    public class DetailViewModel : BindableBase, INavigationAware
    {
        public ReactivePropertySlim<string> TitleText { get; } = new();
        public ReactivePropertySlim<string> DescriptionText { get; } = new();
        public ReactivePropertySlim<string> VersionText { get; } = new();
        public ReactivePropertySlim<string> AuthorText { get; } = new();
        public ReactivePropertySlim<string> LastUpdateText { get; } = new();
        public ReactivePropertySlim<BitmapImage> TitleImage { get; } = new();
        public DelegateCommand PlayCommand { get; }
        public DelegateCommand ExitCommand { get; }
        private readonly IRegionManager _regionManager;
        private readonly ILogger _logger;
        private string _exePath;

        public DetailViewModel(IRegionManager regionManager, IDetail detail, ILogger logger)
        {
            _regionManager = regionManager;
            _logger = logger;
            ExitCommand = new DelegateCommand(detail.HideDetail);
            PlayCommand = new DelegateCommand(Play);
        }

        private async void Play()
        {
            var result = RunApplicationService.Run(_exePath);
            if (!result.IsSuccess)
            {
                _logger.Log(LogLevel.Error, result.ErrorMessage);
                return;
            }

            // ゲーム起動時，1秒後に自動的に閉じる
            await Task.Delay(1000);
            ExitCommand.Execute();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue<GameMetadata>("data", out var data))
            {
                TitleText.Value = data.Title;
                DescriptionText.Value = data.Description;
                VersionText.Value = $"Ver. {data.Version}";
                AuthorText.Value = $"制作: {data.Author}";
                LastUpdateText.Value = $"最終更新: {data.LastUpdate}";
                _exePath = Path.Join(FilePaths.GamePath, data.DirName, data.Title, data.ExeName);
                DisplayTag(data.Tags);
                var img = ImageProvider.GetImage(data);
                if (img.IsSuccess) TitleImage.Value = img.Value;
            }
        }

        private void DisplayTag(List<string> tags)
        {
            foreach (var tag in tags)
            {
                _regionManager.RequestNavigate("TagRegion", nameof(Tag), new NavigationParameters()
                {
                    { "tag", tag },
                    { "fontSize", 30 }
                });
            }
        }

        // 再利用しないのでfalse
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
