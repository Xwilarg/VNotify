using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace VNotify.ViewModels
{
    public class ApiKeyWindowViewModel : ReactiveObject
    {
        public ApiKeyWindowViewModel()
        {
            Validate = ReactiveCommand.CreateFromTask(async () =>
            {
                await SaveApiKeyAndClose.Handle(Unit.Default);
            });
        }

        public string Title { get; } = "Enter your API Key:";
        public string SubTitle { get; } = "You can find it on https://holodex.net/login";

        public ICommand Validate { get; }
        public Interaction<Unit, Unit> SaveApiKeyAndClose { get; } = new();
    }
}
