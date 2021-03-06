using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using VNotify.Application.ViewModels;
using VNotify.Common.IO;

namespace VNotify.Application.Views
{
    public partial class ApiKeyWindow : ReactiveWindow<ApiKeyWindowViewModel>
    {
        public ApiKeyWindow()
        {
            AvaloniaXamlLoader.Load(this);
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
            SaveData.Save();

            ViewModel.InvokeOnCompletion();

            Close();

            context.SetOutput(Unit.Default);
        }
    }
}
