using NotificationIconSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VNotify.Common;

namespace VNotify.Service
{
    public class BackgroundService
    {
        public BackgroundService()
        {
            // Setup
            NotificationManager.Initialize("com.app.vnotify", "VNotify", _iconPath);
            NotificationManager.NotificationIconSelectedEvent += NotificationClicked;

            // Check for API key
            var saveData = SaveData.Load();
            if (saveData.ApiKey == null)
            {
                NotificationManager.SendNotification("VNotify", "Your API key is not set!", null, _iconPath);
                IsAlive = false;
                return;
            }

            // Init HTTP Client
            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Add("X-APIKEY", saveData.ApiKey);
            NotificationManager.SendNotification("VNotify", "VNotify is now started!", null, _iconPath);

            // Init tray
            _tray = new NotificationIcon(_iconPath);
            _tray.NotificationIconSelected += NotificationMenuSelected;
            _checkTimer = new Timer(new TimerCallback(GetCurrentVideos), null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        /// <summary>
        /// Callback when a notification is clicked
        /// </summary>
        /// <param name="streamId">ID of the YouTube video</param>
        private static void NotificationClicked(string streamId)
        {
            if (streamId == "default") // We clicked a notification that isn't about a stream
            {
                return;
            }

            // From https://github.com/AvaloniaUI/Avalonia/blob/89f27089876de14b67298b16da1679eeac1d0cd4/src/Avalonia.Dialogs/AboutAvaloniaDialog.xaml.cs
            var url = $"https://www.youtube.com/watch?v={streamId}";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var escapedArgs = $"xdg-open {url}".Replace("\"", "\\\"");

                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "/bin/sh",
                        Arguments = $"-c \"{escapedArgs}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                );
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? url : "open",
                    Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? $"{url}" : "",
                    CreateNoWindow = true,
                    UseShellExecute = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                });
            }
        }

        /// <summary>
        /// When we press the notification in the tray, display a menu
        /// </summary>
        private void NotificationMenuSelected(NotificationIcon icon)
        {
            if (icon.MenuItems.Count > 0) return;

            var setTextMenuItem = new NotificationMenuItem("Exit");
            setTextMenuItem.NotificationMenuItemSelected += MenuItemExit;

            icon.AddMenuItem(setTextMenuItem);
        }

        /// <summary>
        /// Exit button in the tray, stop the application
        /// </summary>
        private void MenuItemExit(NotificationMenuItem menuItem)
        {
            IsAlive = false;
        }

        /// <summary>
        /// Set on a timer, get the latest videos
        /// </summary>
        private void GetCurrentVideos(object _)
        {
            // Get videos from Holodex API
            var videos = JsonSerializer.Deserialize<Video[]>(_httpClient.GetStringAsync("https://holodex.net/api/v2/videos?status=upcoming&type=stream&order=asc&include=live_info").GetAwaiter().GetResult());
            foreach (var video in videos)
            {
                // If we didn't already check this video
                if (!_awaitingVideos.ContainsKey(video.id))
                {
                    var now = DateTime.Now;
                    var tsDelay = video.start_scheduled - now;
                    var delay = tsDelay.TotalMilliseconds;

                    if (delay > 0) // Video didn't start yet
                    {
                        Console.WriteLine($"[{now.Hour}:{now.Minute}:{now.Second}] Live from {video.channel.name} in {(int)tsDelay.TotalHours} hours, {tsDelay.Minutes} minutes and {tsDelay.Seconds} seconds");
                        _awaitingVideos.Add(video.id, video);
                        _ = Task.Run(async () =>
                        {
                            // Wait until it starts and display a message
                            await Task.Delay((int)delay);
                            NotificationManager.SendNotification("New stream from " + video.channel.name, video.title + "\nClick the notification to open your browser", video.id, _iconPath);
                            _awaitingVideos.Remove(video.id);
                        });
                    }
                }
            }
        }

        // Update method
        public void DoMessageLoop()
        {
            _tray.DoMessageLoop(true);
        }

        // Contains all infos about streams that didn't start yet
        private readonly Dictionary<string, Video> _awaitingVideos = new();

        private NotificationIcon _tray;
        private HttpClient _httpClient;
        private Timer _checkTimer;
        private string _iconPath = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + "../../../tray.ico";
        public bool IsAlive { private set; get; } = true;
    }
}
