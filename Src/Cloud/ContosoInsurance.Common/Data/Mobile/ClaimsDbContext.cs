using Microsoft.Azure.Mobile.Server.Tables;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ContosoInsurance.Common.Data.Mobile
{
    public class ClaimsDbContext: DbContext
    {
        private const string connectionStringName = "Name=MobileClaims";

        public ClaimsDbContext() : base(connectionStringName) { }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
               new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                   "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
            base.OnModelCreating(modelBuilder);
        }
    }
}