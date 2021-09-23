using ReactiveUI;
using VNotify.Common.IO;

namespace VNotify.Application.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            _data = SaveData.Load();
        }

        public bool IsApiKeyLoaded()
            => _data.ApiKey != null;

        private readonly SaveData _data;

        public string TrayReady { get; } = "The tray is now ready to be run";
    }
}
