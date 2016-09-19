namespace ContosoInsurance.Common.Data.Migrations
{
    using Microsoft.Azure.Mobile.Server.Tables;
    using System.Data.Entity.Migrations;

    internal sealed class MobileClaimsConfiguration : DbMigrationsConfiguration<Mobile.ClaimsDbContext>
    {
        public MobileClaimsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Data\Migrations";
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }
    }
}