using ContosoInsurance.Common.Utils;
using System.Linq;
using System.Web.Mvc;

namespace ContosoInsurance.MVC.Helper
{
    public class ApplicationInsightsAttribute : ActionFilterAttribute
    {
        public string Description { get; set; }

        public string IdParamName { get; set; }

        private string CorrelationId { get; set; }

        public ApplicationInsightsAttribute(string description)
        {
            this.Description = description;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var client = ApplicationInsights.CreateTelemetryClient();

            if (filterContext.Exception == null)
            {
                var status = IsSuccess(filterContext.HttpContext.Response.StatusCode)
                    ? OperationStatus.Success
                    : OperationStatus.Failure;
                client.TrackWebAppStatus(CorrelationId, Description, status);
            }
            else
                client.TrackWebAppException(CorrelationId, filterContext.Exception);

            client.Flush();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(IdParamName))
            {
                CorrelationId = string.Empty;
                return;
            }
            if (filterContext.ActionParameters.ContainsKey(IdParamName))
            {
                 var id = filterContext.ActionParameters[IdParamName];
                CorrelationId = id == null ? string.Empty : id.ToString(); 
            }
        }

        private bool IsSuccess(int statusCode)
        {
            var codeString = statusCode.ToString();
            var firstChar = codeString.ToCharArray().First();
            if (firstChar == '4' || firstChar == '5')
                return false;
            else
                return true;         
        }
    }
}