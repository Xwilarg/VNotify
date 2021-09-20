using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VNotify.ViewModels;

namespace VNotify.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(_ =>
            {
                if (!ViewModel!.AreDataLoaded())
                {
                    new ApiKeyWindow()
                    {
                        ViewModel = new ApiKeyWindowViewModel()
                    }.Show(this);
                }
            });
        }
    }
}
