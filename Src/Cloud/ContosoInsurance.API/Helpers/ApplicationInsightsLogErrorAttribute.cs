using ContosoInsurance.Common;
using ContosoInsurance.Common.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace ContosoInsurance.API.Helpers
{
    public class ApplicationInsightsLogErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public Task ExecuteExceptionFilterAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var correlationId = actionExecutedContext.ActionContext.ActionArguments.GetValue(Constants.CorrelationIdKey, string.Empty) as string;
            return Task.Run(() => LogException(correlationId, actionExecutedContext.Exception));
        }

        private void LogException(string correlationId, Exception exception)
        {
            var client = ApplicationInsights.CreateTelemetryClient();
            client.TrackRestAPIException(correlationId, exception);
            client.Flush();
        }
    }
}