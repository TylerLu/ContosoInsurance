using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using System;
using Android.Content;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using Gcm.Client;

namespace ContosoInsurance.Droid
{
    [Activity (Label = "Contoso Insurance", Icon = "@drawable/icon", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static MainActivity instance;
        public const string HOCKEYAPP_APPID = "8e7c354ae6d34dc7bacfc5033f4a88d1";

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            App.UIContext = this;
            LoadApplication(new ContosoInsurance.App());

            instance = this;

            // Register the crash manager before Initializing the trace writer
            CrashManager.Register(this, HOCKEYAPP_APPID);

            //Register to with the Update Manager
            UpdateManager.Register(this, HOCKEYAPP_APPID);

            MetricsManager.Register(Application, HOCKEYAPP_APPID);
            MetricsManager.EnableUserMetrics();

            try
            {
                // Check to ensure everything's setup right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Register for push notifications
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public static MainActivity DefaultService
        {
            get { return instance; }
        }

        private void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}

