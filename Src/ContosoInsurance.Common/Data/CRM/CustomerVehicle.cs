using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoInsurance.Common.Data.CRM
{
    public class CustomerVehicle
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public string LicensePlate { get; set; }

        public string VIN { get; set; }

        public string ImageURL { get; set; }
    }
}