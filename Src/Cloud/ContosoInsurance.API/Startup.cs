using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ContosoInsurance.API.Startup))]

namespace ContosoInsurance.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}