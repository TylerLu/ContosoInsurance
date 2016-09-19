using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContosoInsurance.MVC.Startup))]
namespace ContosoInsurance.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
