using NotificationIconSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
            NotificationManager.Initialize("com.app.vnotify", "VNotify", _iconPath);

            var saveData = SaveData.Load();
            if (saveData.ApiKey == null)
            {
                NotificationManager.SendNotification("VNotify", "Your API key is not set!", "ActionId", _iconPath);
                IsAlive = false;
                return;
            }

            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Add("X-APIKEY", saveData.ApiKey);
            NotificationManager.SendNotification("VNotify", "VNotify is now started!", "ActionId", _iconPath);

            _tray = new NotificationIcon(_iconPath);
            _tray.NotificationIconSelected += NotificationIconSelected;

            _checkTimer = new Timer(new TimerCallback(GetCurrentVideos), null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        private void NotificationIconSelected(NotificationIcon icon)
        {
            if (icon.MenuItems.Count > 0) return;

            var setTextMenuItem = new NotificationMenuItem("Exit");
            setTextMenuItem.NotificationMenuItemSelected += MenuItemExit;

            icon.AddMenuItem(setTextMenuItem);
        }

        private void MenuItemExit(NotificationMenuItem menuItem)
        {
            IsAlive = false;
        }

        private void GetCurrentVideos(object _)
        {
            var videos = JsonSerializer.Deserialize<Video[]>(_httpClient.GetStringAsync("https://holodex.net/api/v2/videos?status=upcoming&type=stream&order=asc&include=live_info").GetAwaiter().GetResult());
            foreach (var video in videos)
            {
                if (!_awaitingVideos.ContainsKey(video.id))
                {
                    var tsDelay = video.start_scheduled - DateTime.UtcNow;
                    var delay = tsDelay.TotalMilliseconds;

                    if (delay > 0)
                    {
                        Console.WriteLine($"Live from {video.channel.name} in {(int)tsDelay.TotalHours} hours, {tsDelay.Minutes} minutes and {tsDelay.Seconds} seconds");
                        _awaitingVideos.Add(video.id, video);
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay((int)delay);
                            NotificationManager.SendNotification("New stream from " + video.channel.name, video.title + "\nhttps://www.youtube.com/watch?v=" + video.id, "ActionId", _iconPath);
                            _awaitingVideos.Remove(video.id);
                        });
                    }
                }
            }
        }

        public void DoMessageLoop()
        {
            _tray.DoMessageLoop(true);
        }

        private Dictionary<string, Video> _awaitingVideos = new();

        private NotificationIcon _tray;
        private HttpClient _httpClient;
        private Timer _checkTimer;
        private string _iconPath = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + "../../../tray.ico";
        public bool IsAlive { private set; get; } = true;
    }
}
