using System;
using System.Diagnostics;
using Xamarin.Forms;
using ContosoInsurance.Helpers;
using HockeyApp;

namespace ContosoInsurance.Views
{
	public partial class LoginView : ContentPage
	{
        public LoginView ()
		{
			InitializeComponent ();
            
            this.loginButton.Clicked += LoginButton_Clicked;
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                //using (var scope = new ActivityIndicatorScope(activityIndicator, activityIndicatorPanel, true))
                {
                    MobileServiceHelper.msInstance.InitMobileService();
                    await MobileServiceHelper.msInstance.DoLoginAsync();

                    var vehiclesView = new VehiclesListView();
                    NavigationPage.SetHasBackButton(vehiclesView, false);
                    await Navigation.PushAsync(vehiclesView, true);
                    
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException("Failure to log in - ", ex);
                //await MobileServiceHelper.msInstance.ClearCachAsync();
                await DisplayAlert("Error", "Login Failure. " + ex.Message, "Close");
            }
        }
        
        private async void SettingsBtn_Tapped(object sender, EventArgs e)
        {
            var settingsView = new SettingsView();
            NavigationPage.SetHasBackButton(settingsView, Device.OS == TargetPlatform.Android);
            await Navigation.PushAsync(settingsView, false);
        }
    }
}
