using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace ContosoInsurance
{
    public class Settings
    {
        static Settings settings;
        public static Settings Current
        {
            get { return settings ?? (settings = new Settings()); }
        }

        public static bool IsDefaultServiceUrl()
        {
            return Current.MobileAppUrl == DefaultMobileAppUrl;
        }

        public string MobileAppUrl
        {
            get { return AppSettings.GetValueOrDefault<string>(MobileAppUrlKey, DefaultMobileAppUrl); }

            set { AppSettings.AddOrUpdateValue<string>(MobileAppUrlKey, value); }
        }
        private const string MobileAppUrlKey = nameof(MobileAppUrlKey);
        public const string DefaultMobileAppUrl = "https://contosoinsurance-api.azurewebsites.net";

        public string MobileHockeyAppId
        {
            get { return AppSettings.GetValueOrDefault<string>(HockeyAppIdKey, DefaultHockeyAppId); }

            set { AppSettings.AddOrUpdateValue<string>(HockeyAppIdKey, value); }
        }
        private const string HockeyAppIdKey = nameof(HockeyAppIdKey);
        public const string DefaultHockeyAppId = "8e7c354ae6d34dc7bacfc5033f4a88d1";


        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
    }
}
