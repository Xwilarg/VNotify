using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using VNotify.Common.IO;

namespace VNotify.Application.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public bool IsApiKeyLoaded()
            => SaveData.Load().ApiKey != null;

        public bool AreChannelDataLoaded()
            => Configuration.Load().Channels.Length != 0;

        public string TrayReady { get; } = "The tray is now ready to be run";

        public string VtuberName
        {
            set
            {
                DisplaySuggestions.Handle(value).GetAwaiter().GetResult();
            }
        }

        public Interaction<string, Unit> DisplaySuggestions { get; } = new();
    }
}
