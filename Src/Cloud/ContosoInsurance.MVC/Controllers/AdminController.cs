using ContosoInsurance.Common.Services;
using ContosoInsurance.Common.Utils;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContosoInsurance.MVC.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> WipeClaims()
        {
            string message;

            var service = new AdminService();
            try
            {
                await service.WipeClaimsAsync();
                message = "All claims have been wiped.";
            }
            catch (Exception ex)
            {
                message = "Failed to wipe all claims. " + ex.Message;

                var telemetryClient = ApplicationInsights.CreateTelemetryClient();
                telemetryClient.TrackWebAppException(ex);
                telemetryClient.Flush();
            }

            var result = new { message = message };
            return Json(result);
        }
    }
}