using ContosoInsurance.Common;
using ContosoInsurance.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContosoInsurance.MVC.Helper
{
    public class ApplicationInsightsLogErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var client = ApplicationInsights.CreateTelemetryClient();
            client.TrackWebAppException(string.Empty, filterContext.Exception);
            client.Flush();
        }
        
    }
}