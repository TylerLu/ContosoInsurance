using System;
using Xamarin.Forms;
using Microsoft.WindowsAzure.MobileServices.Eventing;
using ContosoInsurance.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using HockeyApp;


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



        public static void TraceException(string logEvent, Exception ex)
        {
            Debug.WriteLine(logEvent + ex.Message);
            Trace.WriteLine(logEvent + ex);


            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { "LogType", "Error Log"},
                { "Version", Assembly.GetCallingAssembly().GetName().Version.ToString()},
                { "Description", logEvent + ex.Message}
            };
            /*Hockey APP*/
            MetricsManager.TrackEvent("Failure", properties, null);
        }
        public static void TraceStatus(string logEvent)
        {
            Debug.WriteLine(logEvent);
            Trace.WriteLine(logEvent);

            Dictionary<string, string> properties = new Dictionary<string, string>()
            {
                { "LogType", "Status Log"},
                { "Version", Assembly.GetCallingAssembly().GetName().Version.ToString()},
                { "Description", logEvent},
                { "Status", "Success"}
            };
            /*Hockey APP*/
            MetricsManager.TrackEvent(logEvent, properties, null);
        }
    }
}
