using System;
using Microsoft.WindowsAzure.MobileServices;

namespace ContosoInsurance.Models
{
    public class Claim
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string OtherPartyMobilePhone { get; set; }

        public int VehicleId { get; set; }

        [Version]
        public string Version { get; set; }
    }
}
