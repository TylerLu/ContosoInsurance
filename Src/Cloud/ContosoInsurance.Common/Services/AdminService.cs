using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CRM = ContosoInsurance.Common.Data.CRM;
using Mobile = ContosoInsurance.Common.Data.Mobile;

namespace ContosoInsurance.Common.Services
{
    public class AdminService
    {
        public async Task WipeClaimsAsync()
        {
            var images = new List<string>();

            using (var dbContext = new Mobile.ClaimsDbContext())
            {
                var claims = dbContext.Claims.ToArray();
                dbContext.Claims.RemoveRange(claims);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new CRM.ClaimsDbContext())
            {
                var claims = await dbContext.Claims
                    .Include(i => i.Images)
                    .ToArrayAsync();
                var otherParties = await dbContext.OtherParties
                    .ToArrayAsync();

                foreach (var claim in claims)
                    images.AddRange(claim.Images.Select(i => i.ImageUrl));
                foreach (var otherParty in otherParties)
                {
                    images.Add(otherParty.LicensePlateImageUrl);
                    images.Add(otherParty.InsuranceCardImageUrl);
                    images.Add(otherParty.DriversLicenseImageUrl);
                }

                dbContext.Claims.RemoveRange(claims);
                dbContext.OtherParties.RemoveRange(otherParties);
                await dbContext.SaveChangesAsync();
            }

            var storageAccount = CloudStorageAccount.Parse(AppSettings.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            foreach (var image in images)
            {
                var blob = await blobClient.GetBlobReferenceFromServerAsync(new Uri(image));
                if (await blob.ExistsAsync()) await blob.DeleteAsync();
            }
        }
    }
}