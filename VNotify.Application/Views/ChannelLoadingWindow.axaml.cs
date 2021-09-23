using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
                var vm = ViewModel;
                _ = Task.Run(async () =>
                {
                    await vm.LoadVtuberInfo(); // TODO: is somehow called twice??
                    Close(); // TODO: Not closing the window
                });
            };
        }
    }
}
