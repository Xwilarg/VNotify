using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace VNotify.Application.ViewModels
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

        public void InvokeOnCompletion()
        {
            OnCompletion?.Invoke(this, new());
        }

        public string Title { get; } = "Enter your API Key:";
        public string SubTitle { get; } = "You can find it on https://holodex.net/login";

        public ICommand Validate { get; }
        public Interaction<Unit, Unit> SaveApiKeyAndClose { get; } = new();

        public event EventHandler OnCompletion;
    }
}
