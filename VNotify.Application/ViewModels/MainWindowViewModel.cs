using ReactiveUI;
using VNotify.Common.IO;

namespace VNotify.Application.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            _data = SaveData.Load();
            _config = Configuration.Load();
        }

        public bool IsApiKeyLoaded()
            => _data.ApiKey != null;

        public bool AreChannelDataLoaded()
            => _config.Channels.Length == 0;

        private readonly SaveData _data;
        private readonly Configuration _config;

        public string TrayReady { get; } = "The tray is now ready to be run";
    }
}
