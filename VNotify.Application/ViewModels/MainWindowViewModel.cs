using ReactiveUI;
using VNotify.Common;

namespace VNotify.Application.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            _data = SaveData.Load();
        }

        public bool AreDataLoaded()
            => _data.ApiKey != null;

        private readonly SaveData _data;
    }
}
