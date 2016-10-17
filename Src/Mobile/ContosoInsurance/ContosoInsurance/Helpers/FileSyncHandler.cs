using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Files.Metadata;
using Microsoft.WindowsAzure.MobileServices.Files.Sync;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace ContosoInsurance.Helpers
{
    public class FileSyncHandler : IFileSyncHandler
    {
        private readonly MobileServiceHelper mobileServiceHelper;

        public FileSyncHandler(MobileServiceHelper helper)
        {
            this.mobileServiceHelper = helper;
        }

        public Task<IMobileServiceFileDataSource> GetDataSource(MobileServiceFileMetadata metadata)
        {
            IPlatform platform = DependencyService.Get<IPlatform>();
            return platform.GetFileDataSource(metadata);
        }

        public async Task ProcessFileSynchronizationAction(MobileServiceFile file, FileSynchronizationAction action)
        {
            try {
                if (action == FileSynchronizationAction.Delete) {
                    await FileHelper.DeleteLocalFileAsync(file, mobileServiceHelper.DataFilesPath);
                }
                else { // Create or update - download large format image by looking for 'lg' in the StoreUri parameter
                    Trace.WriteLine(string.Format("File - storeUri: {1}", file.Name, file.StoreUri));
                    await this.mobileServiceHelper.DownloadFileAsync(file);
                }
            }
            catch (Exception ex) { // should catch WrappedStorageException, but this type is internal in the Storage SDK!
                Utils.TraceException("Exception while downloading blob, blob probably does not exist: ", ex);
            }
        }
    }
}
