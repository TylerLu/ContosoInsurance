using Microsoft.Azure;
using System;

namespace ContosoInsurance.Common
{
    public static class AppSettings
    {
        public static readonly string StorageConnectionString = CloudConfigurationManager.GetSetting("MS_AzureStorageAccountConnectionString");

        public static readonly string ClaimManualApproverUrl = CloudConfigurationManager.GetSetting("Contoso_ClaimManualApproverUrl");

        public static readonly string ApplicationInsightsInstrumentationKey = CloudConfigurationManager.GetSetting("MS_ApplicationInsightsInstrumentationKey");

        public static readonly bool AutoSeedUserData = CloudConfigurationManager.GetSetting("AutoSeedUserData").IgnoreCaseEqualsTo("true");

        public const int BlobReadExpireMinitues = 5;

        public const string AADClaimNameType = "name";

        public static class Queues
        {
            public static string MobileClaims = "mobile-claims";
        }
    }
}
