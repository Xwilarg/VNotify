using ReactiveUI;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNotify.Common;
using VNotify.Common.IO;

namespace VNotify.Application.ViewModels
{
    public class ChannelLoadingWindowViewModel : ReactiveObject
    {
        public string IntroMessage { get; } = "Loading information about vtuber, please wait";
        public string LoadingProgress { private set; get; } = _loadingMessage;

        private readonly static string _loadingMessage = "Vtuber information loaded: ";//TODO: info are not updated

        public async Task LoadVtuberInfo()
        {
            int i = 0;
            List<Channel> allChannels = new();
            var apiClient = new ApiClient(SaveData.Load().ApiKey);

            Channel[] latestQuery;
            do
            {
                latestQuery = await apiClient.GetChannelsAsync(i++);
                allChannels.AddRange(latestQuery);
                LoadingProgress = _loadingMessage + allChannels.Count;
            } while (latestQuery.Length == 100);
            var config = Configuration.Load();
            config.Channels = allChannels.ToArray();
            Configuration.Save();
        }
    }
}
