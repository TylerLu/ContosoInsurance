using Microsoft.Azure;

public static class Settings
{
    public static readonly string PreProcessorClaimUrl = CloudConfigurationManager.GetSetting("Contoso_PreProcessorClaimUrl");
    public static readonly string ClaimAutoApproverUrl = CloudConfigurationManager.GetSetting("Contoso_ClaimAutoApproverUrl");
    public static readonly string ClaimsAdjusterEmail = CloudConfigurationManager.GetSetting("Contoso_ClaimsAdjusterEmail");
    public static readonly string ClaimDetailsPageBaseUrl = CloudConfigurationManager.GetSetting("Contoso_ClaimDetailsPageBaseUrl");
    public static readonly string MSVisionServiceSubscriptionKey = CloudConfigurationManager.GetSetting("MS_VisionServiceSubscriptionKey");
    public static readonly string ApplicationInsightsInstrumentationKey = CloudConfigurationManager.GetSetting("MS_ApplicationInsightsInstrumentationKey");
    public static readonly string FunctionsExtensionVersion = CloudConfigurationManager.GetSetting("FUNCTIONS_EXTENSION_VERSION");
}