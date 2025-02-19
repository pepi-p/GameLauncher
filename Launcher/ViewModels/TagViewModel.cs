using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;

namespace Launcher.ViewModels
{
    public class TagViewModel : BindableBase, INavigationAware
    {
        public ReactivePropertySlim<string> Text { get; } = new();
        public ReactivePropertySlim<int> FontSize { get; } = new();

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.TryGetValue<string>("tag", out var tag))
            {
                Text.Value = tag;
            }
            if (navigationContext.Parameters.TryGetValue<int>("fontSize", out var fontSize))
            {
                FontSize.Value = fontSize;
            }
        }

        // 再利用しないのでfalse
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
