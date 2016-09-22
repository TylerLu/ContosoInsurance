using ContosoInsurance.Common;
using ContosoInsurance.Common.Utils;
using System.Web.Http.Filters;

namespace ContosoInsurance.API.Helpers
{
    public class ApplicationInsightsAttribute : ActionFilterAttribute
    {
        public string Description { get; set; }

        public ApplicationInsightsAttribute(string description)
        {
            this.Description = description;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null) return;

            var correlationId = actionExecutedContext.ActionContext.ActionArguments[Constants.CorrelationIdKey] as string;
            var client = ApplicationInsights.CreateTelemetryClient();
            var status = actionExecutedContext.Response.IsSuccessStatusCode ? OperationStatus.Success : OperationStatus.Failure;
            client.TrackRestAPIStatus(correlationId, Description, status);
            client.Flush();
        }
    }
}