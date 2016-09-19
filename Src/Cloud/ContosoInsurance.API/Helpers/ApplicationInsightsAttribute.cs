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
            var correlationId = actionExecutedContext.ActionContext.ActionArguments[Constants.CorrelationIdKey] as string;
            var client = ApplicationInsights.CreateTelemetryClient();

            if (actionExecutedContext.Exception == null)
            {
                var status = actionExecutedContext.Response.IsSuccessStatusCode
                    ? OperationStatus.Success
                    : OperationStatus.Failure;
                client.TraceRestAPIStatus(correlationId, Description, status);
            }
            else
                client.TraceRestAPIException(correlationId, actionExecutedContext.Exception);

            client.Flush();
        }
    }
}