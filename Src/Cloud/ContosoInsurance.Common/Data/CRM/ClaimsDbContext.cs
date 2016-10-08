using Microsoft.Azure.Mobile.Server.Tables;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ContosoInsurance.Common.Data.CRM
{
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

            modelBuilder.Conventions.Add(
               new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                   "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
            base.OnModelCreating(modelBuilder);
        }
    }
}