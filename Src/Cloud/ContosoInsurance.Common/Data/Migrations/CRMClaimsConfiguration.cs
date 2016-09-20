namespace ContosoInsurance.Common.Data.Migrations
{
    using Microsoft.Azure.Mobile.Server.Tables;
    using System.Data.Entity.Migrations;

    public sealed class CRMClaimsConfiguration : DbMigrationsConfiguration<CRM.ClaimsDbContext>
    {
        public CRMClaimsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = @"Data\Migrations";
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }
    }
}