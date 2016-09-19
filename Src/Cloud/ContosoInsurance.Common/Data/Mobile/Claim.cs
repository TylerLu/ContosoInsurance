using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace ContosoInsurance.Common.Data.Mobile
{
    public class Claim : EntityData
    {
        [StringLength(255)]
        public string UserId { get; set; }

        public int VehicleId { get; set; }

        public DateTime DateTime { get; set; }

        [JsonIgnore]
        public DbGeography Coordinates
        {
            get
            {
                if (Longitude == null || Latitude == null) return null;

                var wellKnownText = $"POINT({Longitude.Value} {Latitude.Value})";
                return DbGeography.FromText(wellKnownText);
            }
            set
            {
                if (value != null)
                {
                    Longitude = value.Longitude;
                    Latitude = value.Latitude;
                }
                else
                {
                    Longitude = null;
                    Latitude = null;
                }
            }
        }

        [NotMapped]
        public double? Longitude { get; set; }

        [NotMapped]
        public double? Latitude { get; set; }

        public string OtherPartyMobilePhone { get; set; }

        public string Description { get; set; }
    }
}