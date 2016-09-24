using AutoMapper;
using ContosoInsurance.Common;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using CRM = ContosoInsurance.Common.Data.CRM;
using Mobile = ContosoInsurance.Common.Data.Mobile;

namespace ContosoInsurance.API.Helpers
{
    public class SeedDataHelper
    {
        private static readonly string ImageContainer = "vehicle-images";

        #region Customer Data
        private static readonly CRM.Customer CustomerData = new CRM.Customer
        {
            Street = "123 Somewhere Street",
            City = "New York",
            State = "NY",
            Zip = "10001",
            MobilePhone = "607-555-1212",
            DriversLicenseNumber = "RL8972635",
            DOB = new DateTime(1975, 1, 11),
            PolicyId = "CIC6749726354",
            PolicyStart = new DateTime(2012, 5, 1),
            PolicyEnd = new DateTime(2018, 5, 1)
        };

        private static readonly VehicleData[] VehicleDataArray =
        {
            new VehicleData
            {
                LicensePlate = "TSB-782",
                VIN = "PFS78364572019836",
                ImagePath = @"~\DemoData\vehicle-demo1.png"
            },
            new VehicleData
            {
                LicensePlate = "CBM-243",
                VIN = "JYP73640900281632",
                ImagePath = @"~\DemoData\vehicle-demo2.png"
            }
        };
        #endregion

        private IMapper mapper;

        public SeedDataHelper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CRM.Customer, CRM.Customer>();
                cfg.CreateMap<VehicleData, CRM.CustomerVehicle>();
                cfg.CreateMap<VehicleData, Mobile.Vehicle>();
            });
            this.mapper = config.CreateMapper();
        }

        public async Task<bool> IsCustomerExistedAsync(string userId)
        {
            using (var dbContext = new CRM.ClaimsDbContext())
                return await dbContext.Customers.AnyAsync(i => i.UserId == userId);
        }

        public async Task SeedDataAsync(string userId, string firstName, string lastName, string email)
        {
            var vehicles = await UploadVehicleImagesAsync();
            await SeedCRMClaimsDbAsync(userId, firstName, lastName, email, vehicles);
            await SeedMobileClaimsDbAsync(userId, vehicles);
        }

        private async Task<Vehicle[]> UploadVehicleImagesAsync()
        {
            var storageAccount = CloudStorageAccount.Parse(AppSettings.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ImageContainer);

            var vehicles = new List<Vehicle>();
            foreach (var vehicleData in VehicleDataArray)
            {
                var vehicleGuid = Guid.NewGuid();
                var blockBlob = container.GetBlockBlobReference("vehicle-" + vehicleGuid);
                var path = HttpContext.Current.Request.MapPath(vehicleData.ImagePath);
                using (var fileStream = File.OpenRead(path))
                    await blockBlob.UploadFromStreamAsync(fileStream);

                vehicles.Add(new Vehicle
                {
                    Id = vehicleGuid,
                    VehicleData = vehicleData,
                    ImageUri = blockBlob.StorageUri.PrimaryUri
                });
            }
            return vehicles.ToArray();
        }

        private async Task SeedCRMClaimsDbAsync(string userId, string firstName, string lastName, string email, Vehicle[] vehicles)
        {
            using (var dbContext = new CRM.ClaimsDbContext())
            {
                var customer = mapper.Map<CRM.Customer>(CustomerData);
                customer.UserId = userId;
                customer.FirstName = firstName;
                customer.LastName = lastName;
                customer.Email = email;

                foreach (var vehicle in vehicles)
                {
                    var customerVehicle = mapper.Map<CRM.CustomerVehicle>(vehicle.VehicleData);
                    customerVehicle.Customer = customer;
                    customerVehicle.ImageURL = vehicle.ImageUri.AbsoluteUri;
                    customer.Vehicles.Add(customerVehicle);
                    vehicle.CustomerVehicle = customerVehicle;
                }

                dbContext.Customers.Add(customer);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task SeedMobileClaimsDbAsync(string userId, Vehicle[] vehicles)
        {
            using (var dbContext = new Mobile.ClaimsDbContext())
            {
                foreach (var item in vehicles)
                {
                    var vehicle = mapper.Map<Mobile.Vehicle>(item.VehicleData);
                    vehicle.Id = item.Id.ToString();
                    vehicle.UserId = userId;
                    vehicle.VehicleId = item.CustomerVehicle.Id;
                    vehicle.Deleted = false;
                    dbContext.Vehicles.Add(vehicle);
                }
                await dbContext.SaveChangesAsync();
            }
        }

        class VehicleData
        {
            public string LicensePlate { get; set; }
            public string VIN { get; set; }
            public string ImagePath { get; set; }
        }

        class Vehicle
        {
            public Guid Id { get; set; }
            public Uri ImageUri { get; set; }
            public VehicleData VehicleData { get; set; }
            public CRM.CustomerVehicle CustomerVehicle { get; set; }
        }
    }
}