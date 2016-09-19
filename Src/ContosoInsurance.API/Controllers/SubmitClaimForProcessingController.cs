using ContosoInsurance.API.Helpers;
using ContosoInsurance.Common;
using Microsoft.Azure.Mobile.Server.Config;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContosoInsurance.API.Controllers
{
    [Authorize, MobileAppController]
    public class SubmitClaimForProcessingController : ApiController
    {
        // Post api/SubmitClaimForProcessing/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [ApplicationInsights("Claim submitted to mobile-claims queue")]
        public async Task Post(string id)
        {
            ActionContext.ActionArguments[Constants.CorrelationIdKey] = id;

            var claim = new { Id = id };
            var message = new CloudQueueMessage(JsonConvert.SerializeObject(claim));
           
            var queue = GetMobileClaimsQueue();
            await queue.AddMessageAsync(message);
        }

        private CloudQueue GetMobileClaimsQueue()
        {
            var storageAccount = CloudStorageAccount.Parse(AppSettings.StorageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            return queueClient.GetQueueReference(AppSettings.Queues.MobileClaims);
        }
    }
}