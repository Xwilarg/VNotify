using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VNotify.Common;
using VNotify.Common.IO;

namespace VNotify.Application.ViewModels
{
    public class ChannelLoadingWindowViewModel : ReactiveObject
    {
        public string IntroMessage { get; } = "Loading information about vtuber, please wait";
        private string _loadingProgress = _loadingMessage + "0";
        public string LoadingProgress
        {
            get => _loadingProgress;
            set
            {
                this.RaiseAndSetIfChanged(ref _loadingProgress, value);
            }
        }

        private readonly static string _loadingMessage = "Vtuber information loaded: ";

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

            OnCompletion?.Invoke(this, new());
        }

        public event EventHandler OnCompletion;
    }
}
