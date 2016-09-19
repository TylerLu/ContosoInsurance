using ContosoInsurance.API.Helpers;
using ContosoInsurance.Common.Data.Mobile;
using Microsoft.Azure.Mobile.Server.Files;
using Microsoft.Azure.Mobile.Server.Files.Controllers;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContosoInsurance.API.Controllers.TableControllers
{
    [Authorize]
    public class VehicleStorageController : StorageController<Vehicle>
    {
        private static readonly string ErrorMessage = "The claim you requested does not exist or you do not have permission to access it.";

        private ClaimsDbContext claimsDbContext;
        private IContainerNameResolver containerNameResolver;

        public VehicleStorageController()
            : base(new DefaultStorageProvider())
        {
            this.claimsDbContext = new ClaimsDbContext();
            this.containerNameResolver = new DefaultContainerNameResolver();
        }

        // return a storage token that can be used for blob upload or download
        [HttpPost]
        [Route("tables/Vehicle/{id}/StorageToken")]
        public async Task<HttpResponseMessage> PostStorageTokenRequest(string id, StorageTokenRequest request)
        {
            var currentUserId = await AuthenticationHelper.GetUserId(Request, User);
            if (!claimsDbContext.Vehicles.Any(i => i.Id == id && i.UserId == currentUserId))
                Request.CreateBadRequestResponse(ErrorMessage);

            var token = await GetStorageTokenAsync(id, request, containerNameResolver);
            return Request.CreateResponse(token);
        }

        /// Get the files associated with this record
        [HttpGet]
        [Route("tables/Vehicle/{id}/MobileServiceFiles")]
        public async Task<HttpResponseMessage> GetFiles(string id)
        {
            var currentUserId = await AuthenticationHelper.GetUserId(Request, User);
            if (!claimsDbContext.Vehicles.Any(i => i.Id == id && i.UserId == currentUserId))
                Request.CreateBadRequestResponse(ErrorMessage);

            var files = await GetRecordFilesAsync(id, containerNameResolver);
            return Request.CreateResponse(files);
        }

        // there's no Delete method, because deletion is handled by deleting the image itself
    }
}