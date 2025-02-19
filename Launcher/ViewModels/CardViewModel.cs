using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Services;
using Launcher.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Launcher.ViewModels
{
    public class CardViewModel : BindableBase, INavigationAware
    {
        public ReactivePropertySlim<string> Title { get; } = new("Title");
        public ReactivePropertySlim<string> RegionName { get; } = new(Guid.NewGuid().ToString());
        public ReactivePropertySlim<BitmapImage> Image { get; } = new();
        public DelegateCommand DisplayDetailCommand { get; }
        private readonly IRegionManager _regionManager;
        private readonly IDetail _detailShow;
        private GameMetadata _data;

        public CardViewModel(IRegionManager regionManager, IDetail detailShow)
        {
            _regionManager = regionManager;
            _detailShow = detailShow;
            DisplayDetailCommand = new DelegateCommand(() => _detailShow.ShowDetail(_data));
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue<GameMetadata>("data", out var data))
            {
                _data = data;
                Title.Value = data.Title;
                DisplayTag(data.Tags);
                Image.Value = null;
                var img = ImageProvider.GetImage(data);
                if (img.IsSuccess) Image.Value = img.Value;
            }
        }

        private void DisplayTag(List<string> tags)
        {
            foreach (var tag in tags)
            {
                _regionManager.RequestNavigate(RegionName.Value, nameof(Tag), new NavigationParameters()
                {
                    { "tag", tag },
                    { "fontSize", 24 }
                });
            }
        }

        // 再利用しないのでfalse
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
