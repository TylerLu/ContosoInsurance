using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using Gcm.Client;

using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text;
using ContosoInsurance.Helpers;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
namespace ContosoInsurance.Droid
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]

    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = new string[] { "705339177819" };
        public const string TAG = "ContosoMoments-GCM";
    }


    [Service]
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }

        public GcmService() : base(PushHandlerBroadcastReceiver.SENDER_IDS)
        { }

        public static async Task RegisterWithMobilePushNotifications()
        {
            MobileServiceClient client = MobileServiceHelper.msInstance.Client;

            if (RegistrationID != null && client != null)
            {
                var push = client.GetPush();

                MainActivity.DefaultService.RunOnUiThread(async () =>
                {
                    try
                    {
                        var gcmBody = new JObject {
                            {
                                "data",
                                new JObject {
                                    { "message", "$(Message)" }
                                }
                            }
                        };
                        var template = new JObject {
                            {
                                "genericMessage",
                                new JObject {
                                    {"body", gcmBody}
                                }
                            }
                        };

                        await push.RegisterAsync(RegistrationID, template);
                        Log.Verbose(PushHandlerBroadcastReceiver.TAG, "NotificationHub registration successful");

                    }
                    catch (Exception ex)
                    {
                        Log.Error(PushHandlerBroadcastReceiver.TAG, "RegisterWithMobilePushNotifications: " + ex.Message);
                    }
                });
            }
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(PushHandlerBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;

            if (registrationId != null)
                RegisterWithMobilePushNotifications();
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info(PushHandlerBroadcastReceiver.TAG, "GCM Message Received!");

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());
            }

            //Store the message
            var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            var edit = prefs.Edit();
            edit.PutString("last_msg", msg.ToString());
            edit.Commit();

            var message = intent.Extras.GetString("message");
            if (!string.IsNullOrEmpty(message))
                createNotification("Contoso Insurance", message);
            else
                createNotification("Contoso Insurance - unknown message details", msg.ToString());
        }

        void createNotification(string title, string desc)
        {
            var builder = new Notification.Builder(Application.Context)
                .SetContentTitle(title)
                .SetContentText(desc)
                .SetSmallIcon(Android.Resource.Drawable.SymActionEmail);
            var notification = builder.Build();

            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(1, notification);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(PushHandlerBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }

        protected override async void OnUnRegistered(Context context, string registrationId)
        {
            MobileServiceClient client = MobileServiceHelper.msInstance.Client;
            var push = client.GetPush();

            await push.UnregisterAsync();
        }
    }
}