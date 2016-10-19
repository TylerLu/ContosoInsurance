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
        internal static string GetUserId(HttpRequestMessage request, IPrincipal user)
        {
            var principal = user as ClaimsPrincipal;
            var provider = principal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider");
            if (provider == null) return "";

            var sid = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            return $"{provider}:{sid}";
        }

        internal static async Task<ProviderCredentials> GetCurrentCredentialAsync(HttpRequestMessage request, IPrincipal user)
        {
            var principal = user as ClaimsPrincipal;
            var claim = principal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider");
            if (claim == null) return null;

            var provider = claim.Value;
            ProviderCredentials creds = null;
            if (provider.IgnoreCaseEqualsTo("microsoftaccount"))
                creds = await user.GetAppServiceIdentityAsync<MicrosoftAccountCredentials>(request);
            else if (provider.IgnoreCaseEqualsTo("facebook"))
                creds = await user.GetAppServiceIdentityAsync<FacebookCredentials>(request);
            else if (provider.IgnoreCaseEqualsTo("aad"))
                creds = await user.GetAppServiceIdentityAsync<AzureActiveDirectoryCredentials>(request);
            return creds;
        }
    }
}