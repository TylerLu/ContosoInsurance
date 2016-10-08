#r "System.ComponentModel.DataAnnotations"

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Spatial;

public static class Mobile
{
    public class ClaimsDbContext : DbContext
    {
        private const string schema = "Mobile";
        private const string connectionStringName = "Name=MobileClaims";

        public ClaimsDbContext() : base(connectionStringName) { }

        public DbSet<Claim> Claims { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>().ToTable("Claims", schema);
        }
    }

    public abstract class EntityData
    {
        public string Id { get; set; }

        public bool Deleted { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public byte[] Version { get; set; }
    }

    public class Claim : EntityData
    {
        public string UserId { get; set; }

        public int VehicleId { get; set; }

        public DateTime DateTime { get; set; }

        public DbGeography Coordinates { get; set; }

        public string OtherPartyMobilePhone { get; set; }

        public string Description { get; set; }
    }
}