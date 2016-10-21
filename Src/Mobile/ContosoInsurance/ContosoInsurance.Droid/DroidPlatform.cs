using Android.Content;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Files.Sync;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Media;
using Microsoft.WindowsAzure.MobileServices;
using Android.Webkit;
using Xamarin.Forms;
using Xamarin.Auth;
using ContosoInsurance.Helpers;
using Xamarin.Geolocation;

[assembly: Xamarin.Forms.Dependency(typeof(ContosoInsurance.Droid.DroidPlatform))]
namespace ContosoInsurance.Droid
{
    public class DroidPlatform : IPlatform
    {

        public async Task DownloadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string fullPath)
        {
            await table.DownloadFileAsync(file, fullPath);
        }

        public async Task<IMobileServiceFileDataSource> GetFileDataSource(MobileServiceFileMetadata metadata)
        {
            var filePath = 
                await FileHelper.GetLocalFilePathAsync(
                    metadata.ParentDataItemId, metadata.FileName, dataFilesPath: await GetDataFilesPath());
            return new PathMobileServiceFileDataSource(filePath);
        }

        public string GetRootDataPath()
        {
            // TODO: Windows needs instead Windows.Storage.ApplicationData.Current.LocalFolder.Path
            return Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        }

        public Task<string> GetDataFilesPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filesPath = Path.Combine(appData, "ContosoImages");

            if (!Directory.Exists(filesPath)) {
                Directory.CreateDirectory(filesPath);
            }

            return Task.FromResult(filesPath);
        }

        public async Task<string> TakePhotoAsync(object context)
        {
            try {
                var uiContext = context as Context;
                if (uiContext != null) {
                    var mediaPicker = new MediaPicker(uiContext);
                    var photo = await mediaPicker.PickPhotoAsync();

                    return photo.Path;
                }
            }
            catch (TaskCanceledException) {
            }

            return null;
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceAuthenticationProvider provider)
        {
            MobileServiceUser user = await MobileServiceHelper.msInstance.Client.LoginAsync(Forms.Context, provider);
            return user;
        }

        public void ClearCache()
        {
            CookieManager.Instance.RemoveAllCookie();
        }


        public AccountStore GetAccountStore()
        {
            return AccountStore.Create(Forms.Context);
        }

        public async Task<Position> GetGeolocator(object context)
        {
            var uiContext = context as Context;
            var locator = new Geolocator(uiContext) { DesiredAccuracy = 50 };
            Position position = await locator.GetPositionAsync(timeout: 10000);
            return position;
        }

        public async Task RegisterWithMobilePushNotifications()
        {
            GcmService.RegisterWithMobilePushNotifications();
        }
    }
}