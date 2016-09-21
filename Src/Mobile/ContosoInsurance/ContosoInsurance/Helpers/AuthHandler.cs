using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoInsurance.Helpers
{
    class AuthHandler : DelegatingHandler
    {
        public IMobileServiceClient Client { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.Client == null) {
                throw new InvalidOperationException("Make sure to set the 'Client' property in this handler before using it.");
            }

            // Cloning the request, in case we need to send it again
            var clonedRequest = await CloneRequestAsync(request);
            var response = await base.SendAsync(clonedRequest, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized) {
                try {
                    await MobileServiceHelper.msInstance.Client.RefreshUserAsync();

                    clonedRequest = await CloneRequestAsync(request);

                    clonedRequest.Headers.Remove("X-ZUMO-AUTH");
                    clonedRequest.Headers.Add("X-ZUMO-AUTH", Client.CurrentUser.MobileServiceAuthenticationToken);

                    // Resend the request
                    response = await base.SendAsync(clonedRequest, cancellationToken);
                }
                catch (InvalidOperationException) {
                    // user cancelled auth, so return the original response
                    return response;
                }
            }

            return response;
        }
        private async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
        {
            var result = new HttpRequestMessage(request.Method, request.RequestUri);
            foreach (var header in request.Headers) {
                result.Headers.Add(header.Key, header.Value);
            }

            if (request.Content != null && request.Content.Headers.ContentType != null) {
                var requestBody = await request.Content.ReadAsStringAsync();
                var mediaType = request.Content.Headers.ContentType.MediaType;
                result.Content = new StringContent(requestBody, Encoding.UTF8, mediaType);
                foreach (var header in request.Content.Headers) {
                    if (!header.Key.Equals("Content-Type", StringComparison.OrdinalIgnoreCase)) {
                        result.Content.Headers.Add(header.Key, header.Value);
                    }
                }
            }

            return result;
        }
    }
}
