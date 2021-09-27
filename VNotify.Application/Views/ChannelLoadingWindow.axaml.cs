using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using System.Threading.Tasks;
using VNotify.Application.ViewModels;

namespace VNotify.Application.Views
{
    public partial class ChannelLoadingWindow : ReactiveWindow<ChannelLoadingWindowViewModel>
    {
        public ChannelLoadingWindow()
        {
            AvaloniaXamlLoader.Load(this);

            this.Activated += (sender, e) =>
            {
                ViewModel.LoadVtuberInfo().ContinueWith((_) =>
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        Close();
                    });
                });
            };
        }
    }
}
