using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VNotify.ViewModels;

namespace VNotify.Views
{
    public partial class ApiKeyWindow : ReactiveWindow<ApiKeyWindowViewModel>
    {
        public ApiKeyWindow()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
        }
    }
}
