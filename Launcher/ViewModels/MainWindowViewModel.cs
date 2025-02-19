using Launcher.Interfaces;
using Launcher.Models;
using Launcher.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace Launcher.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<int> Height { get; } = new(1030);
        public ReactiveProperty<string> LogText { get; } = new();
        public DelegateCommand UpdateCommand { get; }
        private readonly IRegionManager _regionManager;
        private int _cardCount = 0;
        private Storyboard _showDetailAnimation;
        private Storyboard _hideDetailAnimation;
        private Storyboard ShowDetailAnimation
        {
            get
            {
                if (_showDetailAnimation is null) _showDetailAnimation = Application.Current.Windows[0].FindResource("ShowDetailAnimation") as Storyboard;
                return _showDetailAnimation;
            }
        }
        private Storyboard HideDetailAnimation
        {
            get
            {
                if (_hideDetailAnimation is null)
                {
                    _hideDetailAnimation = Application.Current.Windows[0].FindResource("HideDetailAnimation") as Storyboard;
                    _hideDetailAnimation.Completed += (sender, args) => _regionManager.Regions["DetailRegion"].RemoveAll();
                }
                return _hideDetailAnimation;
            }
        }

        public MainWindowViewModel(IRegionManager regionManager, ILoggerListener loggerListener, IGameLoader gameLoader, IDetailListener detailShow, IDataRepositoryListener dataRepository)
        {
            _regionManager = regionManager;
            loggerListener.OnLogReceived += (s) => LogText.Value = s;
            dataRepository.OnDataChanged += AllDisplayCard;
            detailShow.OnDetailShow += ShowDetail;
            detailShow.OnDetailHide += HideDetail;
            UpdateCommand = new DelegateCommand(() => gameLoader.UpdateAllGame());
        }

        private void ShowDetail(GameMetadata data)
        {
            ShowDetailAnimation.Begin();

            _regionManager.RequestNavigate("DetailRegion", nameof(Detail), new NavigationParameters()
            {
                { "data", data }
            });
        }

        private void HideDetail()
        {
            HideDetailAnimation.Begin();
        }

        private void AllDisplayCard(Dictionary<int, GameMetadata> datas)
        {
            _cardCount = 0;
            _regionManager.Regions["CardRegion"].RemoveAll();

            foreach (var data in datas)
            {
                AddDisplayCard(data.Value);
            }
        }

        /// <summary>
        /// 表示させるカードを追加する
        /// </summary>
        /// <param name="data">ゲームデータ</param>
        private void AddDisplayCard(GameMetadata data)
        {
            _regionManager.RequestNavigate("CardRegion", nameof(Card), new NavigationParameters()
            {
                { "data", data }
            });

            // Canvasの高さの更新
            _cardCount++;
            UpdateCanvasHeight(_cardCount);
        }

        /// <summary>
        /// カードの枚数に応じてCanvasの高さを変更する
        /// </summary>
        private void UpdateCanvasHeight(int cardCount)
        {
            int height = 496 * (cardCount / 3 + 1);
            Height.Value = height > 1030 ? height : 1030;
        }
    }
}
