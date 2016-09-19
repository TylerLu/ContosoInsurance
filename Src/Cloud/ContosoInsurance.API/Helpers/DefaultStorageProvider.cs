using ContosoInsurance.Common;
using Microsoft.Azure.Mobile.Server.Files;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoInsurance.API.Helpers
{
    public class DefaultStorageProvider : AzureStorageProvider
    {
        public static readonly string IncidentImagesContainer =
            $"{ImageKinds.Incident}-{DefaultContainerNameResolver.DefaultContainerSuffix}";
        
        public static readonly int MaxIncidentImageCount = 5;

        public DefaultStorageProvider(string connectionString)
            : base(connectionString) { }

        public DefaultStorageProvider()
            : base(AppSettings.StorageConnectionString) { }

        protected override async Task<IEnumerable<CloudBlockBlob>> GetContainerFilesAsync(string containerName)
        {
            var containerInfo = containerName.Split('/');
            var blobContainerName = containerInfo[0];
            var fileName = containerInfo[1];

            var blobContainer = GetContainer(blobContainerName);
            if (blobContainer == null) return Enumerable.Empty<CloudBlockBlob>();

            return await GetContainerFilesAsync(blobContainer, fileName);
        }

        private async Task<IEnumerable<CloudBlockBlob>> GetContainerFilesAsync(CloudBlobContainer blobContainer, string fileName)
        {
            var list = new List<CloudBlockBlob>();
            if (blobContainer.Name == IncidentImagesContainer)
            {
                for (int i = 1; i <= MaxIncidentImageCount; i++)
                {
                    var blobName = $"{fileName}-{i:d2}";
                    var blob = blobContainer.GetBlockBlobReference(blobName);
                    if (await blob.ExistsAsync()) list.Add(blob);
                }
            }
            else
            {
                var blob = blobContainer.GetBlockBlobReference(fileName);
                if (await blob.ExistsAsync()) list.Add(blob);
            }
            return list;
        }
    }
}