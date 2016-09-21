using System;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices.Eventing;
using ContosoInsurance.Models;

namespace ContosoInsurance.Helpers
{
    public class ImageDownloadEvent : MobileServiceEvent
    {
        public string Id { get; set; }

        public ImageDownloadEvent(string id) : base(id)
        {
            this.Id = id;
        }        
    }

    public class SyncCompletedEvent : MobileServiceEvent
    {
        public static SyncCompletedEvent Instance = new SyncCompletedEvent();

        public SyncCompletedEvent() : base("SyncCompletedEvent") { }
    }

    public class ActivityIndicatorScope : IDisposable
    {
        private ActivityIndicator indicator;
        private Grid indicatorPanel;

        public ActivityIndicatorScope(ActivityIndicator indicator, Grid indicatorPanel, bool showIndicator)
        {
            this.indicator = indicator;
            this.indicatorPanel = indicatorPanel;

            SetIndicatorActivity(showIndicator);
        }

        private void SetIndicatorActivity(bool isActive)
        {
            this.indicator.IsVisible = isActive;
            this.indicator.IsRunning = isActive;
            this.indicatorPanel.IsVisible = isActive;
        }

        public void Dispose()
        {
            SetIndicatorActivity(false);
        }
    }

    public static class Utils
    {       
        public static bool IsOnline()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            return networkConnection.IsConnected;
        }
    }
}
