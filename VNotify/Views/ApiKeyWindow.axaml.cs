using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using VNotify.Models;
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
            this.WhenActivated(_ =>
            {
                ViewModel!.SaveApiKeyAndClose.RegisterHandler(SaveApiKeyAndClose);
            });
        }

        public void SaveApiKeyAndClose(InteractionContext<Unit, Unit> context)
        {
            var apiKey = this.FindControl<TextBox>("ApiKey").Text;
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return;
            }

            var data = SaveData.Load();
            data.ApiKey = apiKey;
            data.Save();
            Close();

            context.SetOutput(Unit.Default);
        }
    }
}
