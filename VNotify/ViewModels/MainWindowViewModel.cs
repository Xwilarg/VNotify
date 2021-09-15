using ReactiveUI;
using VNotify.Models;

namespace VNotify.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            _data = SaveData.Load();
        }

        public bool AreDataLoaded()
            => _data.ApiKey != null;

        private SaveData _data;
    }
}
