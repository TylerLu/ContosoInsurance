using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoInsurance.Common.Data.CRM
{
    public class Customer
    {
        public Customer()
        {
            this.Vehicles = new HashSet<CustomerVehicle>();
        }

        public int Id { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public string DriversLicenseNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }

        public string PolicyId { get; set; }

        [Column(TypeName = "date")]
        public DateTime PolicyStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime PolicyEnd { get; set; }

        public virtual ISet<CustomerVehicle> Vehicles { get; set; }
    }
}