using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using VNotify.Application.ViewModels;
using VNotify.Common.IO;

namespace VNotify.Application.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);

            this.WhenActivated(_ =>
            {
                ViewModel!.DisplaySuggestions.RegisterHandler(DisplaySuggestions);

                if (!ViewModel!.IsApiKeyLoaded()) // API key is not loaded yet
                {
                    var vm = new ApiKeyWindowViewModel();
                    vm.OnCompletion += (sender, e) => // When we are done loading it, we need to take care of the "channel loading" part
                    {
                        new ChannelLoadingWindow()
                        {
                            ViewModel = new ChannelLoadingWindowViewModel()
                        }.Show(this);
                    };
                    new ApiKeyWindow()
                    {
                        ViewModel = vm
                    }.Show(this);
                }
                else if (!ViewModel!.AreChannelDataLoaded()) // Channel data not loaded
                {
                    new ChannelLoadingWindow()
                    {
                        ViewModel = new ChannelLoadingWindowViewModel()
                    }.Show(this);
                }
            });
        }

        public void DisplaySuggestions(InteractionContext<string, Unit> context)
        {
            var input = context.Input.ToUpperInvariant();

            var panel = this.FindControl<StackPanel>("Suggestions");
            List<IControl> controls = new();
            foreach (var channel in Configuration.Load().Channels
                .OrderBy(x => {
                    var enDistance = LevenshteinDistance(input, x.english_name?.ToUpperInvariant());
                    var jpDistance = LevenshteinDistance(input, x.name?.ToUpperInvariant());
                    return Math.Min(enDistance, jpDistance);
                })
                .Take(5)
            )
            {
                var button = new Button
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Width = 300,
                    Content = channel.english_name ?? channel.name
                };
                controls.Add(button);
            }
            panel.Children.Clear();
            panel.Children.AddRange(controls);

            context.SetOutput(Unit.Default);
        }

        // https://gist.github.com/Davidblkx/e12ab0bb2aff7fd8072632b396538560
        private static int LevenshteinDistance(string source1, string source2)
        {
            if (source1 == null || source2 == null)
                return int.MaxValue;

            var source1Length = source1.Length;
            var source2Length = source2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (source2[j - 1] == source1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[source1Length, source2Length];
        }
    }
}
