using ContosoInsurance.Common;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ContosoInsurance.MVC.Utils
{
    public class BlobUtil
    {
        public static CloudBlobContainer GetContainer(string storageName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AppSettings.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(storageName);
            return container;
        }

        public static string GetBlobSasUri(string containerName,string blobName)
        {
            var container = GetContainer(containerName);
            //Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            //Set the expiry time and permissions for the blob.
            //In this case the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            var exireFactor = AppSettings.BlobReadExpireMinitues;
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1 * exireFactor);
            sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(exireFactor);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }

        public static string ConverToBlobSas(string originalBlobUrl)
        {
            if (string.IsNullOrEmpty(originalBlobUrl))
                return string.Empty;
            var imgFields = originalBlobUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return BlobUtil.GetBlobSasUri(imgFields[imgFields.Length - 2], imgFields[imgFields.Length - 1]);
        }
    }
}