using ContosoInsurance.API.Helpers;
using ContosoInsurance.Common;
using ContosoInsurance.Common.Data.Mobile;
using Microsoft.Azure.Mobile.Server;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace ContosoInsurance.API.Controllers
{
    [Authorize]
    public class ClaimController : TableController<Claim>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            ClaimsDbContext context = new ClaimsDbContext();
            DomainManager = new EntityDomainManager<Claim>(context, Request);
        }

        // GET tables/Claim
        public async Task<IQueryable<Claim>> GetAllClaim()
        {
            var currentUserId = await AuthenticationHelper.GetUserIdAsync(Request, User);
            return Query().Where(i => i.UserId == currentUserId);
        }

        // GET tables/Claim/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<SingleResult<Claim>> GetClaim(string id)
        {
            var queryable = (await GetAllClaim()).Where(i => i.Id == id);
            return SingleResult.Create(queryable);
        }

        // POST tables/Claim
        [ApplicationInsights("Claim received from mobile app")]
        public async Task<IHttpActionResult> PostClaim(Claim item)
        {
            ActionContext.ActionArguments[Constants.CorrelationIdKey] = item.Id;

            var currentUserId = await AuthenticationHelper.GetUserIdAsync(Request, User);
            item.UserId = currentUserId;

            Claim current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // PATCH tables/Claim/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public async Task<Claim> PatchClaim(string id, Delta<Claim> patch)
        {
            if (!(await GetClaim(id)).Queryable.Any()) return null;
            return await UpdateAsync(id, patch);
        }

        //// DELETE tables/Claim/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task DeleteClaim(string id)
        //{
        //     return DeleteAsync(id);
        //}
    }
}
