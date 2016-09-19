using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Hosting;

namespace ContosoInsurance.Common.Utils
{
    public enum OperationStatus
    {
        Failure = 0,
        Success = 1,
    }

    public static class TelemetryClientExtension
    {
        public static void TraceStatus(this TelemetryClient client, string eventName, string correlationId, string description, OperationStatus status, IDictionary<string, double> metrics = null)
        {
            var properties = GetCommonProperties("Status Log", correlationId);
            properties["Description"] = description;
            properties["Status"] = status.ToString();
            client.TrackEvent(eventName, properties, metrics);
        }

        public static void TraceException(this TelemetryClient client, string logName, string correlationId, Exception ex)
        {
            var properties = GetCommonProperties("Error Log", correlationId);
            properties["LogName"] = logName;
            client.TrackException(ex, properties);
        }

        public static void TraceRestAPIStatus(this TelemetryClient client, string correlationId, string description, OperationStatus status)
        {
            var metrics = new Dictionary<string, double> { { "REST API", 0 } };
            TraceStatus(client, "REST API Status", correlationId, description, status, metrics);
        }

        public static void TraceWebAppStatus(this TelemetryClient client, string correlationId, string description, OperationStatus status)
        {
            var metrics = new Dictionary<string, double> { { "Web App", 0 } };
            TraceStatus(client, "Web App Status", correlationId, description, status, metrics);
        }

        public static void TraceRestAPIException(this TelemetryClient client, string correlationId, Exception ex)
        {
            TraceException(client, "REST API Error", correlationId, ex);
        }

        public static void TraceWebAppException(this TelemetryClient client, string correlationId, Exception ex)
        {
            TraceException(client, "Web App Error", correlationId, ex);
        }


        private static Dictionary<string, string> GetCommonProperties(string logType, string correlationId)
        {
            return new Dictionary<string, string>()
            {
                { "LogType", logType},
                { "Host",  HostingEnvironment.ApplicationHost.GetSiteName() },
                { "CorrelationId", correlationId },
                { "Version", Assembly.GetCallingAssembly().GetName().Version.ToString() }
            };
        }

    }
}