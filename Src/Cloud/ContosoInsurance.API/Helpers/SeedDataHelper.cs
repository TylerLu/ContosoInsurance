using ContosoInsurance.Common;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CRM = ContosoInsurance.Common.Data.CRM;
using Mobile = ContosoInsurance.Common.Data.Mobile;

namespace ContosoInsurance.API.Helpers
{
    public class SeedDataHelper
    {
        private static readonly string ImageContainer = "vehicle-images";
        private static readonly string[] Images = { @"~\DemoData\vehicle-demo1.png", @"~\DemoData\vehicle-demo2.png" };

        public bool IsCustomerExisted(string userId)
        {
            using (var dbContext = new CRM.ClaimsDbContext())
                return dbContext.Customers.Any(i => i.UserId == userId);
        }

        public async Task SeedAsync(string userId, string firstName, string lastName, string email)
        {
            var vehicleImages = await UploadVehicleImagesAsync();
            await SeedCRMClaimsDbAsync(userId, firstName, lastName, email, vehicleImages);
            await SeedMobileClaimsDbAsync(userId, vehicleImages);
        }

        private async Task<VehicleImage[]> UploadVehicleImagesAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(AppSettings.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ImageContainer);

            var vehicleImages = new List<VehicleImage>();
            foreach (var image in Images)
            {
                var vehicleImage = new VehicleImage { VehicleGuid = Guid.NewGuid() };
                var fileName = "vehicle-" + vehicleImage.VehicleGuid;
                var blockBlob = container.GetBlockBlobReference(fileName);
                var path = HttpContext.Current.Request.MapPath(image);
                using (var fileStream = File.OpenRead(path))
                    await blockBlob.UploadFromStreamAsync(fileStream);
                vehicleImage.ImageUri = blockBlob.StorageUri.PrimaryUri;
                vehicleImages.Add(vehicleImage);
            }
            return vehicleImages.ToArray();
        }

        private async Task SeedCRMClaimsDbAsync(string userId, string firstName, string lastName, string email, VehicleImage[] images)
        {
            using (var dbContext = new CRM.ClaimsDbContext())
            {
                var customer = new CRM.Customer()
                {
                    UserId = userId,
                    FirstName = firstName,
                    LastName = lastName,
                    Street = "123 Somewhere Street",
                    City = "New York",
                    State = "NY",
                    Zip = "10001",
                    Email = email,
                    MobilePhone = "607-555-1212",
                    DriversLicenseNumber = "RL8972635",
                    DOB = new DateTime(1975, 1, 11),
                    PolicyId = "CIC6749726354",
                    PolicyStart = new DateTime(2012, 5, 1),
                    PolicyEnd = new DateTime(2018, 5, 1)
                };
                customer.Vehicles.Add(new CRM.CustomerVehicle()
                {
                    Customer = customer,
                    LicensePlate = "TSB-782",
                    VIN = "PFS78364572019836",
                    ImageURL = images[0].ImageUri.AbsoluteUri
                });
                customer.Vehicles.Add(new CRM.CustomerVehicle()
                {
                    Customer = customer,
                    LicensePlate = "CBM-243",
                    VIN = "JYP73640900281632",
                    ImageURL = images[1].ImageUri.AbsoluteUri

                });

                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();

                images[0].VehicleId = customer.Vehicles.First().Id;
                images[1].VehicleId = customer.Vehicles.Skip(1).First().Id;
            }
        }

        private async Task SeedMobileClaimsDbAsync(string userId, VehicleImage[] images)
        {
            using (var dbContext = new Mobile.ClaimsDbContext())
            {
                var vehicle1 = new Mobile.Vehicle()
                {
                    Id = images[0].VehicleGuid.ToString(),
                    UserId = userId,
                    VehicleId = images[0].VehicleId,
                    LicensePlate = "TSB-782",
                    VIN = "PFS78364572019836",
                    Deleted = false,
                };

                var vehicle2 = new Mobile.Vehicle()
                {
                    Id = images[1].VehicleGuid.ToString(),
                    UserId = userId,
                    VehicleId = images[1].VehicleId,
                    LicensePlate = "CBM-243",
                    VIN = "JYP73640900281632",
                    Deleted = false,
                };

                dbContext.Vehicles.Add(vehicle1);
                dbContext.Vehicles.Add(vehicle2);
                await dbContext.SaveChangesAsync();
            }
        }

        class VehicleImage
        {
            public int VehicleId { get; set; }
            public Guid VehicleGuid { get; set; }
            public Uri ImageUri { get; set; }
        }
    }
}