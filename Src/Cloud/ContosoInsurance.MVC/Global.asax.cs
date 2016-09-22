using ContosoInsurance.Common.Data.CRM;
using ContosoInsurance.Common.Data.Migrations;
using ContosoInsurance.MVC.Helper;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ContosoInsurance.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterFilters();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ClaimsDbContext, CRMClaimsConfiguration>());
        }

        private static void RegisterFilters()
        {
            GlobalFilters.Filters.Add(new ApplicationInsightsLogErrorAttribute());
            GlobalFilters.Filters.Add(new JsonHandlerAttribute());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
