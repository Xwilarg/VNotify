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
                if (!ViewModel!.IsApiKeyLoaded())
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
