using ReactiveUI;

namespace VNotify.Application.ViewModels
{
    public class ChannelLoadingWindowViewModel : ReactiveObject
    {
        public string IntroMessage { get; } = "Loading information about vtuber, please wait";
    }
}
