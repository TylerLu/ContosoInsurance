using Microsoft.Azure.Mobile.Server;

namespace ContosoInsurance.Common.Data.Mobile
{
    public class Vehicle : EntityData
    {
        public string UserId { get; set; }

        public string LicensePlate { get; set; }

        public string VIN { get; set; }

        public int VehicleId { get; set; }
    }
}