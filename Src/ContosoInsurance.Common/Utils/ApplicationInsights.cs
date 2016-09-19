using Microsoft.ApplicationInsights;

namespace ContosoInsurance.Common.Utils
{
    public static class ApplicationInsights
    {
        public static TelemetryClient CreateTelemetryClient()
        {
            var telemetryClient = new TelemetryClient();
            telemetryClient.InstrumentationKey = AppSettings.ApplicationInsightsInstrumentationKey;
            return telemetryClient;
        }
    }
}