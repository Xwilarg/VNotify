using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VNotify.Application.ViewModels;

namespace VNotify.Application.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);

            this.WhenActivated(_ =>
            {
                if (!ViewModel!.IsApiKeyLoaded()) // API key is not loaded yet
                {
                    var vm = new ApiKeyWindowViewModel();
                    vm.OnCompletion += (sender, e) => // When we are done loading it, we need to take care of the "channel loading" part
                    {
                        new ChannelLoadingWindow()
                        {
                            ViewModel = new ChannelLoadingWindowViewModel()
                        }.Show(this);
                    };
                    new ApiKeyWindow()
                    {
                        ViewModel = vm
                    }.Show(this);
                }
                else if (!ViewModel!.AreChannelDataLoaded()) // Channel data not loaded
                {
                    new ChannelLoadingWindow()
                    {
                        ViewModel = new ChannelLoadingWindowViewModel()
                    }.Show(this);
                }
            });
        }
    }
}
