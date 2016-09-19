using System;
using Microsoft.Azure.Mobile.Server.Authentication;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ContosoInsurance.API.Helpers
{
    public static class AuthenticationHelper
    {
        internal static async Task<string> GetUserId(HttpRequestMessage request, IPrincipal user)
        {
            var principal = user as ClaimsPrincipal;
            var claim = principal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider");
            if (claim == null) return string.Empty;

            var provider = claim.Value;
            ProviderCredentials creds = null;
            if (provider.IgnoreCaseEqualsTo("microsoftaccount"))
                creds = await user.GetAppServiceIdentityAsync<MicrosoftAccountCredentials>(request);
            else if (provider.IgnoreCaseEqualsTo("facebook"))
                creds = await user.GetAppServiceIdentityAsync<FacebookCredentials>(request);
            else if (provider.IgnoreCaseEqualsTo("aad"))
                creds = await user.GetAppServiceIdentityAsync<AzureActiveDirectoryCredentials>(request);

            return creds != null ?
                string.Format("{0}:{1}", creds.Provider,  creds.Claims[ClaimTypes.NameIdentifier]) :
                null;
        }
    }
}