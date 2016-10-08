#r "System.ComponentModel.DataAnnotations"

#load "OCRAttribute.csx"

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Spatial;

public static class CRM
{
    public static class ImageKind
    {
        public const string LicensePlate = "LicensePlate";
        public const string InsuranceCard = "InsuranceCard";
        public const string DriverLicense = "DriverLicense";
    }

    public class ClaimsDbContext : DbContext
    {
        private const string schema = "CRM";
        private const string connectionStringName = "Name=CRMClaims";

        public ClaimsDbContext() : base(connectionStringName) { }

        public DbSet<Claim> Claims { get; set; }

        public DbSet<ClaimImage> ClaimImages { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<CustomerVehicle> CustomerVehicles { get; set; }

        public DbSet<OtherParty> OtherParties { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>().ToTable("Claims", schema);
            modelBuilder.Entity<ClaimImage>().ToTable("ClaimImages", schema);
            modelBuilder.Entity<Customer>().ToTable("Customers", schema);
            modelBuilder.Entity<CustomerVehicle>().ToTable("CustomerVehicles", schema);
            modelBuilder.Entity<OtherParty>().ToTable("OtherParties", schema);

            modelBuilder.Entity<Claim>()
                .HasMany(i => i.Images)
                .WithRequired(i => i.Claim)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Customer>()
                .HasMany(i => i.Vehicles)
                .WithRequired(i => i.Customer)
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }
    }

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

    public enum ClaimDamageAssessment
    {
        Minimal = 0,
        Moderate = 1,
        Severe = 2
    }

    public class ClaimImage
    {
        public int Id { get; set; }

        public int ClaimId { get; set; }

        [ForeignKey("ClaimId")]
        public virtual Claim Claim { get; set; }

        public string ImageUrl { get; set; }
    }

    public enum ClaimStatus
    {
        Submitted = 0,
        AutoApproved = 1,
        AutoRejected = 2,
        ManualApproved = 3,
        ManualRejected = 4
    }

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

    public class OtherParty
    {
        //Ideally, the OCR attributes defined in this class would be read from a data file
        //so code does not need to be re-compiled to adjust them. To keep the sample simple
        //they are defined here.
        //Keep in mind, these OCR attributes are configured to work with the sample OCR
        //images provided with this sample.  If you wish to use different images you must
        //adjust these OCR coordinates to work with the images you supply.
        public int Id { get; set; }

        [OCR(ImageKind.DriverLicense, 200, 189, 200, 20, @"^\w+")]
        public string FirstName { get; set; }

        [OCR(ImageKind.DriverLicense, 200, 173, 200, 20)]
        public string LastName { get; set; }

        [OCR(ImageKind.DriverLicense, 200, 207, 200, 20)]
        public string Street { get; set; }

        [OCR(ImageKind.DriverLicense, 200, 225, 200, 20, @"^\w+")]
        public string City { get; set; }

        [OCR(ImageKind.DriverLicense, 200, 225, 200, 20, @"(?<=\w+\s+)\w+")]
        public string State { get; set; }

        [OCR(ImageKind.DriverLicense, 200, 225, 200, 20, @"\d+$")]
        public string Zip { get; set; }

        [Column(TypeName = "date")]
        [OCR(ImageKind.DriverLicense, 248, 245, 70, 20)]
        public DateTime? DOB { get; set; }

        public string MobilePhone { get; set; }

        [OCR(ImageKind.InsuranceCard, 5, 78, 82, 22)]
        public string PolicyId { get; set; }

        [Column(TypeName = "date")]
        [OCR(ImageKind.InsuranceCard, 134, 78, 76, 22)]
        public DateTime? PolicyStart { get; set; }

        [Column(TypeName = "date")]
        [OCR(ImageKind.InsuranceCard, 264, 78, 76, 22)]
        public DateTime? PolicyEnd { get; set; }

        [OCR(ImageKind.DriverLicense, 200, 142, 321, 131, @"[0-9 ]+")]
        public string DriversLicenseNumber { get; set; }

        [OCR(ImageKind.LicensePlate, 144, 104, 280, 132)]
        public string LicensePlate { get; set; }

        [OCR(ImageKind.InsuranceCard, 185, 198, 200, 23)]
        public string VIN { get; set; }

        public string LicensePlateImageUrl { get; set; }

        public string InsuranceCardImageUrl { get; set; }

        public string DriversLicenseImageUrl { get; set; }
    }
}