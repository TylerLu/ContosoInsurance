using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Files.Sync;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Media;
using Microsoft.WindowsAzure.MobileServices;
using Foundation;
//using Facebook.LoginKit;
using UIKit;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Linq;
using ContosoInsurance.Helpers;
using Xamarin.Geolocation;

[assembly: Xamarin.Forms.Dependency(typeof(ContosoInsurance.iOS.TouchPlatform))]
namespace ContosoInsurance.iOS
{
    class TouchPlatform : IPlatform
    {
        public async Task DownloadFileAsync<T>(IMobileServiceSyncTable<T> table, MobileServiceFile file, string fullPath)
        {
            await table.DownloadFileAsync(file, fullPath);
        }

        public async Task<IMobileServiceFileDataSource> GetFileDataSource(MobileServiceFileMetadata metadata)
        {
            var filePath = await FileHelper.GetLocalFilePathAsync(metadata.ParentDataItemId, metadata.FileName, await GetDataFilesPath());
            return new PathMobileServiceFileDataSource(filePath);
        }

        public Task<string> GetDataFilesPath()
        {
            string filesPath = Path.Combine(GetRootDataPath(), "ContosoImages");

            if (!Directory.Exists(filesPath)) {
                Directory.CreateDirectory(filesPath);
            }

            return Task.FromResult(filesPath);
        }

        public string GetRootDataPath()
        {
            // return a reference to <Application_Home>/Library/Caches, so that the images are not marked for iCloud backup
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documents, "..", "Library", "Caches"); 
        }

        public async Task<string> TakePhotoAsync(object context)
        {
            try {
                var mediaPicker = new MediaPicker();
                var mediaFile = await mediaPicker.PickPhotoAsync();
                return mediaFile.Path;
            }
            catch (TaskCanceledException) {
                return null;
            }
        }

        public async Task<MobileServiceUser> LoginAsync(MobileServiceAuthenticationProvider provider)
        {
            var view = GetTopViewController();
            MobileServiceUser user = await MobileServiceHelper.msInstance.Client.LoginAsync(view, provider);
            return user;
        }

        public void ClearCache()
        {
            foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies) {
                NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);
            }
        }

        private UIViewController GetTopViewController()
        {
            var view = UIApplication.SharedApplication.KeyWindow.RootViewController;

            // Find the view controller that's currently on top. This is required if there's a modal page being displayed
            while (view.PresentedViewController != null) {
                view = view.PresentedViewController;
            }

            return view;
        }
        public async Task<Position> GetGeolocator(object context)
        {
            var locator = new Geolocator { DesiredAccuracy = 50 };
            Position position = await locator.GetPositionAsync(timeout: 10000);
            return position;
        }
    }
}