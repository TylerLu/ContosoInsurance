using System;
using Xamarin.Forms;
using ContosoInsurance.Helpers;

namespace ContosoInsurance.Views
{
	public partial class SettingsViewiOS : ContentPage
	{
        public SettingsViewiOS ()
		{
            Title = "Contoso Insurance";
            InitializeComponent ();
            this.settingsURL.Text = Settings.Current.MobileAppUrl;
        }

        public async void SaveClicked(object sender, EventArgs e)
        {
            string newUri = string.Empty;
            if (!GetHttpsUri(settingsURL.Text, out newUri))
            {
                await DisplayAlert("Configuration Error", "Invalid URI entered", "OK");
                return;
            }

            if (Settings.Current.MobileAppUrl == settingsURL.Text || Settings.Current.MobileAppUrl == newUri)
            {
                Settings.Current.MobileAppUrl = newUri;
                await Navigation.PopAsync(false);
            }
            else
            {
                Settings.Current.MobileAppUrl = newUri;
                await MobileServiceHelper.msInstance.DoLogOutAsync();
                await Navigation.PopToRootAsync(false);
            }
        }

        private bool GetHttpsUri(string inputString, out string httpsUri)
        {
            if (!Uri.IsWellFormedUriString(inputString, UriKind.Absolute))
            {
                httpsUri = "";
                return false;
            }

            var uriBuilder = new UriBuilder(inputString)
            {
                Scheme = Uri.UriSchemeHttps,
                Port = -1
            };

            httpsUri = uriBuilder.ToString();
            return true;
        }

        #region User Common Actions

        public async void MenuClicked(object sender, EventArgs e)
        {
            this.menuList.IsVisible = !this.menuList.IsVisible;
        }

        public async void LogoutBtn_Tapped(object sender, EventArgs e)
        {
            this.menuList.IsVisible = !this.menuList.IsVisible;
            await MobileServiceHelper.msInstance.DoLogOutAsync();
            await Navigation.PopToRootAsync(true);
        }

        #endregion
    }
}
