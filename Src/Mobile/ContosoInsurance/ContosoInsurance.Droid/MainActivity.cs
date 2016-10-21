using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using System;
using Android.Content;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;
using Gcm.Client;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Support.V7.App;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace ContosoInsurance.Droid
{
    [Activity (Label = "Contoso Insurance", Icon = "@drawable/icon", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        public static MainActivity instance;
        private string HOCKEYAPP_APPID = Settings.Current.MobileHockeyAppId;

        protected override void OnCreate (Bundle bundle)
        {
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate (bundle);

            this.Window.AddFlags(WindowManagerFlags.Fullscreen);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            App.UIContext = this;
            LoadApplication(new ContosoInsurance.App());

            instance = this;

            Toolbar toolbar = (Toolbar)FindViewById(ContosoInsurance.Droid.Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetHomeButtonEnabled(true);

            toolbar.SetNavigationIcon(ContosoInsurance.Droid.Resource.Drawable.navmenu);
            //toolbar.SetLogo(ContosoInsurance.Droid.Resource.Drawable.navmenu);

            try
            {
                // Register the crash manager before Initializing the trace writer
                CrashManager.Register(this, HOCKEYAPP_APPID);

                //Register to with the Update Manager
                //UpdateManager.Register(this, HOCKEYAPP_APPID);

                MetricsManager.Register(Application, HOCKEYAPP_APPID);
                MetricsManager.EnableUserMetrics();
            }
            catch(Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
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
            Android.Support.V7.App.AlertDialog.Builder builder = new Android.Support.V7.App.AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}

