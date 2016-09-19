using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ContosoInsurance.Common.Data.CRM
{
    public class Claim
    {
        public Claim()
        {
            this.Images = new HashSet<ClaimImage>();
        }

        public int Id { get; set; }
        
        public int VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public virtual CustomerVehicle Vehicle { get; set; }

        public int OtherPartyId { get; set; }

        [ForeignKey("OtherPartyId")]
        public virtual OtherParty OtherParty { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime DueDate { get; set; }

        public DbGeography Coordinates { get; set; }

        public Guid CorrelationId { get; set; }

        public ClaimStatus Status { get; set; }

        public string Type { get; set; }

        public ClaimDamageAssessment? DamageAssessment { get; set; }

        public string Description { get; set; }

        public virtual ISet<ClaimImage> Images { get; set; }
    }
}