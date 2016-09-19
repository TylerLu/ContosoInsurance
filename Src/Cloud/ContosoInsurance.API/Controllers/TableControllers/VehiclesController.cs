using ContosoInsurance.Common.Data.Mobile;
using Microsoft.Azure.Mobile.Server;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace ContosoInsurance.API.Controllers
{
    public class VehiclesController : TableController<CustomerVehicle>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ClaimsDbContext context = new ClaimsDbContext();
            DomainManager = new EntityDomainManager<CustomerVehicle>(context, Request);
        }

        // GET tables/Vehicles
        public IQueryable<CustomerVehicle> GetAllCustomerVehicle()
        {
            return Query(); 
        }

        // GET tables/Vehicles/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<CustomerVehicle> GetCustomerVehicle(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Vehicles/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<CustomerVehicle> PatchCustomerVehicle(string id, Delta<CustomerVehicle> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Vehicles
        public async Task<IHttpActionResult> PostCustomerVehicle(CustomerVehicle item)
        {
            CustomerVehicle current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Vehicles/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCustomerVehicle(string id)
        {
             return DeleteAsync(id);
        }
    }
}