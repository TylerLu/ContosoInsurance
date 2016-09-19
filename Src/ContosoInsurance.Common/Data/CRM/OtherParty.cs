using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoInsurance.Common.Data.CRM
{
    public class OtherParty
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }
        
        public string MobilePhone { get; set; }

        public string PolicyId { get; set; }

        [Column(TypeName="date")]
        public DateTime? PolicyStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PolicyEnd { get; set; }

        public string DriversLicenseNumber { get; set; }

        public string LicensePlate { get; set; }

        public string VIN { get; set; }

        public string LicensePlateImageUrl { get; set; }

        public string InsuranceCardImageUrl { get; set; }

        public string DriversLicenseImageUrl { get; set; }
    }
}