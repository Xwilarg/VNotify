using NotificationIconSharp;
using System;
using System.Net.Http;
using VNotify.Common;

namespace VNotify.Service
{
    public class BackgroundService
    {
        public BackgroundService()
        {
            string iconPath = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + "../../../tray.ico";
            NotificationManager.Initialize("com.app.vnotify", "VNotify", iconPath);

            var saveData = SaveData.Load();
            if (saveData.ApiKey == null)
            {
                NotificationManager.SendNotification("VNotify", "Your API key is not set!", "ActionId", iconPath);
                IsAlive = false;
                return;
            }

            _httpClient = new();
            _httpClient.DefaultRequestHeaders.Add("X-APIKEY", saveData.ApiKey);
            NotificationManager.SendNotification("VNotify", "VNotify is now started!", "ActionId", iconPath);

            _tray = new NotificationIcon(iconPath);
            _tray.NotificationIconSelected += NotificationIconSelected;
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

        public void DoMessageLoop()
        {
            _tray.DoMessageLoop(true);
        }

        private NotificationIcon _tray;
        private HttpClient _httpClient;
        public bool IsAlive { private set; get; } = true;
    }
}
