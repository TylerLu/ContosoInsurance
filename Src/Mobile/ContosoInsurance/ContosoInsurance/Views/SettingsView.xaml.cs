using System;
using Xamarin.Forms;
using ContosoInsurance.Helpers;

namespace ContosoInsurance.Views
{
    public partial class SettingsView : ContentPage
    {
        public SettingsView()
        {
            InitializeComponent();
            this.settingsURL.Text = Settings.Current.MobileAppUrl;
            this.hockeyId.Text = Settings.Current.MobileHockeyAppId;
            if (Device.OS == TargetPlatform.iOS)
            {
                Title = "Contoso Insurance";
                ToolbarItem savebutton = new ToolbarItem
                {
                    Text = "Save",
                    Order = ToolbarItemOrder.Default,
                    Priority = 0
                };
                savebutton.Clicked += SaveClicked;
                ToolbarItem menubutton = new ToolbarItem
                {
                    Icon = "navmenu.png",
                    Order = ToolbarItemOrder.Default,
                    Priority = 0
                };
                menubutton.Clicked += MenuClicked;
                ToolbarItems.Add(savebutton);
                ToolbarItems.Add(menubutton);
            }
            else
            {
                Title = "Settings";
                ToolbarItem returnbutton = new ToolbarItem
                {
                    Icon = "navmenu.png",
                    Order = ToolbarItemOrder.Primary,
                    Priority = 0
                };
                returnbutton.Clicked += MenuClicked;
                ToolbarItems.Add(returnbutton);
            }
        }

        public async void SaveClicked(object sender, EventArgs e)
        {
            string newUri = string.Empty;
            if (!GetHttpsUri(settingsURL.Text, out newUri))
            {
                await DisplayAlert("Configuration Error", "Invalid URI entered", "OK");
                return;
            }
            bool bMobileAppUrlChange = (Settings.Current.MobileAppUrl != newUri);
            bool bHockeyAppIdChange = (Settings.Current.MobileHockeyAppId != hockeyId.Text);

            if (bHockeyAppIdChange)
            {
                Settings.Current.MobileHockeyAppId = hockeyId.Text;
                await DisplayAlert("Configuration Hint", "Restart the App to enable Hockey App.", "OK");
                //return;
            }

            if (bMobileAppUrlChange)
            {
                Settings.Current.MobileAppUrl = newUri;
                await MobileServiceHelper.msInstance.DoLogOutAsync();
                await Navigation.PopToRootAsync(false);
            }
            else
            {
                await Navigation.PopAsync(false);
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
