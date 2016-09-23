using ContosoInsurance.Common;
using System;
using System.Diagnostics;
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
            Trace.TraceError("Enter");
            await base.OnActionExecutingAsync(actionContext, cancellationToken);
            if (IsSwitchOff())
                return;
            var controller = (actionContext.ControllerContext.Controller as ApiController);
            var principal = controller.User;
            var currentUserId = await AuthenticationHelper.GetUserIdAsync(actionContext.ControllerContext.Request, principal);
            var givenname = await AuthenticationHelper.GetGivennameAsync(actionContext.ControllerContext.Request, principal);
            var surname = await AuthenticationHelper.GetSurnameAsync(actionContext.ControllerContext.Request, principal);
            var email = await AuthenticationHelper.GetEmailAsync(actionContext.ControllerContext.Request, principal);
            await CheckSeedDataAsync(currentUserId, givenname, surname, email);
        }

        private bool IsSwitchOff()
        {
            return !AppSettings.AutoSeedUserData.IgnoreCaseEqualsTo("true");
        }

        private async Task CheckSeedDataAsync(string userId, string givenname, string surname, string email)
        {
            var seedDataHelper = new SeedDataHelper(userId, givenname, surname, email);
            await seedDataHelper.TrySeedAsync();
        }
    }
}