using ContosoInsurance.API.Helpers;
using ContosoInsurance.Common.Data.Migrations;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.Azure.Mobile.Server.Tables.Config;
using Owin;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Http.Validation;
using CRM = ContosoInsurance.Common.Data.CRM;
using Mobile = ContosoInsurance.Common.Data.Mobile;

namespace ContosoInsurance.API
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IBodyModelValidator), new CustomBodyModelValidator());

            new MobileAppConfiguration()
                .AddMobileAppHomeController()
                .MapApiControllers()
                .AddTables(
                    new MobileAppTableConfiguration()
                        .MapTableControllers()
                        .AddEntityFramework()
                    )
                //.AddPushNotifications()
                //.MapLegacyCrossDomainController()
                .ApplyTo(config);

            config.Filters.Add(new ApplicationInsightsLogErrorAttribute());

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<CRM.ClaimsDbContext, CRMClaimsConfiguration>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Mobile.ClaimsDbContext, MobileClaimsConfiguration>());            

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will
                    // only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseWebApi(config);
        }
    }
}