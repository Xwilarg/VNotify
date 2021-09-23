using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VNotify.Application.ViewModels;

namespace VNotify.Application.Views
{
    public partial class ChannelLoadingWindow : ReactiveWindow<ChannelLoadingWindowViewModel>
    {
        public ChannelLoadingWindow()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
