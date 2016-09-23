using ContosoInsurance.Common;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ContosoInsurance.API.Helpers
{
    public class AutoSeedDataAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (!AppSettings.AutoSeedUserData) return;

            var controller = actionContext.ControllerContext.Controller as ApiController;
            if (!controller.User.Identity.IsAuthenticated) return;

            var currentUserId = await AuthenticationHelper.GetUserIdAsync(controller.Request, controller.User);
            var helper = new SeedDataHelper();
            if (helper.IsCustomerExisted(currentUserId)) return;

            var creds = await AuthenticationHelper.GetCurrentCredentialAsync(controller.Request, controller.User);

            var firstName = creds.Claims.GetValue(ClaimTypes.GivenName);
            var lastName = creds.Claims.GetValue(ClaimTypes.Surname);
            // TODO: Get email
            await helper.SeedAsync(currentUserId, firstName, lastName, null);
        }
        
    }
}