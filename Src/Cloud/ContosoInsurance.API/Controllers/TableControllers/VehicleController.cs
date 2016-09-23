using ContosoInsurance.API.Helpers;
using ContosoInsurance.Common.Data.Mobile;
using Microsoft.Azure.Mobile.Server;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ContosoInsurance.API.Controllers
{
    [Authorize]
    public class VehicleController : TableController<Vehicle>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ClaimsDbContext context = new ClaimsDbContext();
            DomainManager = new EntityDomainManager<Vehicle>(context, Request);
        }

        // GET tables/Vehicle
        [AutoSeedData]
        public async Task<IQueryable<Vehicle>> GetAllVehicle()
        {
            var currentUserId = await AuthenticationHelper.GetUserIdAsync(Request, User);
            return Query().Where(i => i.UserId == currentUserId);
        }

        // GET tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<SingleResult<Vehicle>> GetVehicle(string id)
        {
            var queryable = (await GetAllVehicle()).Where(i => i.Id == id);
            return SingleResult.Create(queryable);
        }
        
        //// PATCH tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task<Vehicle> PatchVehicle(string id, Delta<Vehicle> patch)
        //{
        //     return UpdateAsync(id, patch);
        //}

        //// POST tables/Vehicle
        //public async Task<IHttpActionResult> PostVehicle(Vehicle item)
        //{
        //    Vehicle current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}

        //// DELETE tables/Vehicle/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task DeleteVehicle(string id)
        //{
        //     return DeleteAsync(id);
        //}
    }
}