using ContosoInsurance.API.Helpers;
using ContosoInsurance.Common.Data.Mobile;
using Microsoft.Azure.Mobile.Server.Files;
using Microsoft.Azure.Mobile.Server.Files.Controllers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContosoInsurance.API.Controllers.TableControllers
{
    public class ClaimStorageController : StorageController<Claim>
    {
        private IContainerNameResolver containerNameResolver;

        public ClaimStorageController()
            : base(new ClaimImageStorageProvider())
        {
            this.containerNameResolver = new ClaimImageContainerNameResolver();
        }
        
        // return a storage token that can be used for blob upload or download
        [HttpPost]
        [Route("tables/Claim/{id}/StorageToken")]
        public async Task<HttpResponseMessage> PostStorageTokenRequest(string id, StorageTokenRequest request)
        {
            var token = await GetStorageTokenAsync(id, request, containerNameResolver);
            return Request.CreateResponse(token);
        }

        /// Get the files associated with this record
        [HttpGet]
        [Route("tables/Claim/{id}/MobileServiceFiles")]
        public async Task<HttpResponseMessage> GetFiles(string id)
        {
            var files = await GetRecordFilesAsync(id, containerNameResolver);
            return Request.CreateResponse(files);
        }

        // there's no Delete method, because deletion is handled by deleting the image itself
    }
}