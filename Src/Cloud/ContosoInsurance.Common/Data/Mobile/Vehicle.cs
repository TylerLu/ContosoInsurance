using Microsoft.Azure.Mobile.Server;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoInsurance.Common.Data.Mobile
{
    [Table("CustomerVehicles")]
    public class Vehicle: EntityData
    {
        public string UserId { get; set; }
        
        public string LicensePlate { get; set; }
        
        public string VIN { get; set; }
        
        public int VehicleId { get; set; }
    }
}