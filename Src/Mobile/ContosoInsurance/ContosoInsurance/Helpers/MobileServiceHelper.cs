using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using PCLStorage;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace ContosoInsurance.Helpers
{
    public class MobileServiceHelper
    {
        public static MobileServiceHelper msInstance;
        public static string QueryVehicleString = "allVehicle";
        public static string QueryClaimString = "allClaim";

        private const string LocalDbFilename = "localDb.sqlite";
        public MobileServiceClient Client;
        public MobileServiceUser AuthenticatedUser;

        public IMobileServiceSyncTable<Models.Vehicle> vehicleTableSync;
        public IMobileServiceSyncTable<Models.Claim> claimTableSync;

        private static Object currentDownloadTaskLock = new Object();
        private static Task currentDownloadTask = Task.FromResult(0);
        public string DataFilesPath { get; set; }

        internal void InitMobileService()
        {
            var authHandler = new AuthHandler();
            Client = new MobileServiceClient(Settings.Current.MobileAppUrl, new LoggingHandler(true), authHandler);
            authHandler.Client = Client;
        }

        internal async Task InitLocalStoreAsync(string userId)
        {
            if (!Client.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore(LocalDbFilename + userId);
                store.DefineTable<Models.Vehicle>();
                store.DefineTable<Models.Claim>();

                // Initialize file sync
                Client.InitializeFileSyncContext(new FileSyncHandler(this), store, new FileSyncTriggerFactory(Client, true));

                // Uses the default conflict handler, which fails on conflict
                await Client.SyncContext.InitializeAsync(store, StoreTrackingOptions.NotifyLocalAndServerOperations);
            }

            vehicleTableSync = Client.GetSyncTable<Models.Vehicle>();
            claimTableSync = Client.GetSyncTable<Models.Claim>();

            IPlatform platform = DependencyService.Get<IPlatform>();
            DataFilesPath = await platform.GetDataFilesPath();
        }

        internal Task DownloadFileAsync(MobileServiceFile file)
        {
            // should only download one file at a time, since it's possible to get duplicate notifications for the same file
            // ContinueWith is used along with Wait() so that only one thread downloads at a time
            lock (currentDownloadTaskLock)
            {
                return currentDownloadTask =
                    currentDownloadTask.ContinueWith(x => DoFileDownloadAsync(file)).Unwrap();
            }
        }

        private async Task DoFileDownloadAsync(MobileServiceFile file)
        {
            Debug.WriteLine("Starting file download - " + file.Name);

            IPlatform platform = DependencyService.Get<IPlatform>();
            var path = await FileHelper.GetLocalFilePathAsync(file.ParentId, file.Name, DataFilesPath);
            var tempPath = Path.ChangeExtension(path, ".temp");
            await platform.DownloadFileAsync(vehicleTableSync, file, tempPath);

            var fileRef = await FileSystem.Current.LocalStorage.GetFileAsync(tempPath);
            await fileRef.RenameAsync(path, NameCollisionOption.ReplaceExisting);
            Debug.WriteLine("Renamed file to - " + path);

            await Client.EventManager.PublishAsync(new ImageDownloadEvent(file.ParentId));
        }

        internal async Task PurgeDataAsync()
        {
            IPlatform platform = DependencyService.Get<IPlatform>();
            await vehicleTableSync.PurgeFilesAsync();
            await vehicleTableSync.PurgeAsync(QueryVehicleString, null, true, CancellationToken.None);
            await claimTableSync.PurgeFilesAsync();
            await claimTableSync.PurgeAsync(QueryClaimString, null, true, CancellationToken.None);

            //delete downloaded files
           await FileHelper.DeleteLocalPathAsync(await platform.GetDataFilesPath());
        }

        public async Task SyncAsync()
        {
            await Client.SyncContext.PushAsync();
            await vehicleTableSync.PullAsync(QueryVehicleString, vehicleTableSync.CreateQuery());
        }
        internal async Task DoLoginAsync()
        {
            var platform = DependencyService.Get<IPlatform>();
            var user = await platform.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
            msInstance.AuthenticatedUser = user;
            Debug.WriteLine("Authenticated with user: " + user.UserId);
            Trace.WriteLine("Authenticated with user: " + user.UserId);
            await InitLocalStoreAsync(user.UserId);
        }

        internal async Task DoLogOutAsync()
        {
            if(Client != null && Client.SyncContext.IsInitialized)
            {
                await PurgeDataAsync();
                var platform = DependencyService.Get<IPlatform>();
                platform.ClearCache();
            
                await Client.LogoutAsync();
            }
        }
        internal async Task ClearCachAsync()
        {
            var platform = DependencyService.Get<IPlatform>();
            platform.ClearCache();
            if (Client != null){
                await Client.LogoutAsync();
            }
        }
    }
}
