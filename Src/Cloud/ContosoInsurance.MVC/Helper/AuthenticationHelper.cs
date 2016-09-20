using ContosoInsurance.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ContosoInsurance.MVC.Helper
{
    public class AuthenticationHelper
    {
        public static string GetUserName()
        {
            ClaimsPrincipal cp = ClaimsPrincipal.Current;
            var nameClaim = cp.FindFirst(AppSettings.AADClaimNameType);
            if (nameClaim != null)
                return nameClaim.Value;
            else
                return HttpContext.Current.User.Identity.Name;
        }
    }
}