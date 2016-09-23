using ContosoInsurance.API.Models;
using ContosoInsurance.Common;
using ContosoInsurance.Common.Data.CRM;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ContosoInsurance.API.Helpers
{
    public class SeedDataHelper
    {
        private const string ImageContainer = "vehicle-images";


        private List<ImageSeed> imageList; //guid & url
        private string userId;

        private ClaimsDbContext crmClaimsDbContext;
        private Common.Data.Mobile.ClaimsDbContext mobileClaimsDbContext;

        private List<Customer> customerList;
        private List<CustomerVehicle> customerVehicleList;

        public bool NeedSeedData { get; private set; }

        public SeedDataHelper(string userId, string givename, string surname, string email)
        {
            this.userId = userId;
            crmClaimsDbContext = new ClaimsDbContext();
            mobileClaimsDbContext = new Common.Data.Mobile.ClaimsDbContext();

            CheckUserId();
            if (!NeedSeedData)
                return;

            imageList = new List<ImageSeed>() {
               new ImageSeed(){ ImageKey = Guid.NewGuid() },
               new ImageSeed(){ ImageKey = Guid.NewGuid() },
            };
            customerList = new List<Customer>() {
                new Customer() {
                        UserId = userId,
                        FirstName =givename,
                        LastName = surname,
                        Street ="123 Somewhere Street",
                        City ="New York",
                        State ="NY",
                        Zip = "10001",
                        Email = email,
                        MobilePhone ="607-555-1212",
                        DriversLicenseNumber = "RL8972635",
                        DOB = new DateTime(1975,1,11),
                        PolicyId = "CIC6749726354",
                        PolicyStart = new DateTime(2012,5,1),
                        PolicyEnd = new DateTime(2018,5,1)
                    }
            };
        }

        private string GetClaimValue(System.Security.Claims.Claim claim)
        {
            if (claim == null)
                return string.Empty;
            if (claim.Value == null)
                return string.Empty;
            return claim.Value;
        }


        private void CheckUserId()
        {
            NeedSeedData = !crmClaimsDbContext.Customers.Any(i => i.UserId == this.userId);
        }

        public async Task TrySeedAsync()
        {
            if (!NeedSeedData)
                return;
            await AddCustomerAsync();
            await UploadImagesAsync();
            await AddCRMCusomterVehicleAsync();
            await AddMobileCusomterVehicleAsync();
        }
        public async Task AddCustomerAsync()
        {
            crmClaimsDbContext.Customers.AddRange(customerList);
            await crmClaimsDbContext.SaveChangesAsync();
        }

        public async Task AddCRMCusomterVehicleAsync()
        {
            customerVehicleList = new List<CustomerVehicle>() {
                new CustomerVehicle() {
                        CustomerId = customerList[0].Id,
                        LicensePlate = "TSB-782",
                        VIN = "PFS78364572019836",
                        ImageURL = imageList[0].BlobUrl
                    },
                    new CustomerVehicle() {
                        CustomerId = customerList[0].Id,
                        LicensePlate = "CBM-243",
                        VIN = "JYP73640900281632",
                        ImageURL = imageList[1].BlobUrl
                    }
            };
            crmClaimsDbContext.CustomerVehicles.AddRange(customerVehicleList);
            await crmClaimsDbContext.SaveChangesAsync();
        }


        public async Task AddMobileCusomterVehicleAsync()
        {
            mobileClaimsDbContext.Vehicles.AddRange(
               new Common.Data.Mobile.Vehicle[] {
                   new Common.Data.Mobile.Vehicle() {
                        Id = imageList[0].ImageKey.ToString(),
                        UserId = this.userId,
                        VehicleId = customerVehicleList[0].Id,
                        LicensePlate = "TSB-782",
                        VIN = "PFS78364572019836",
                        Deleted = false,
                   },
                   new Common.Data.Mobile.Vehicle() {
                        Id = imageList[1].ImageKey.ToString(),
                        UserId = this.userId,
                        VehicleId = customerVehicleList[1].Id,
                        LicensePlate = "CBM-243",
                        VIN = "JYP73640900281632",
                        Deleted = false,
                   },
               }
            );
            await mobileClaimsDbContext.SaveChangesAsync();
        }
        public async Task UploadImagesAsync()
        {
            List<FileStream> result = new List<FileStream>();
            var images = new string[] {
                "vehicle-demo1.png",
                "vehicle-demo2.png",
            };
            for (var i = 0; i < images.Length; i++)
            {
                var localFile = images[i];
                string path = HttpContext.Current.Request.MapPath("~\\DemoData\\" + localFile);
                FileStream fsImage = new FileStream(path, FileMode.Open);
                var image = imageList[i];
                image.BlobUrl = await SaveFileAsync(image.GetBlobName(), fsImage, fsImage.Length);
            }
        }

        private async Task<string> SaveFileAsync(string fileName, Stream fileStream, long fileLength)
        {
            string retUrl = string.Empty;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(AppSettings.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ImageContainer);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            if (!blockBlob.Exists())
            {
                using (fileStream)
                {
                    await blockBlob.UploadFromStreamAsync(fileStream, fileLength);
                    retUrl = blockBlob.StorageUri.PrimaryUri.ToString();
                }
            }
            return retUrl;
        }
    }
}