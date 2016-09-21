using System;
using System.Diagnostics;
using Xamarin.Forms;
using ContosoInsurance.Helpers;

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
            using (var scope = new ActivityIndicatorScope(activityIndicator, activityIndicatorPanel, true))
            {
                try
                {
                    MobileServiceHelper.msInstance.InitMobileService();
                    await MobileServiceHelper.msInstance.DoLoginAsync();

                    var vehiclesView = new VehiclesListViewiOS();
                    await Navigation.PushAsync(vehiclesView, true);
                    NavigationPage.SetHasBackButton(vehiclesView, false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failure to log in - " + ex.Message);
                    Trace.WriteLine("Failure to log in - " + ex);
                    await DisplayAlert("Error", "Login Failure. "+ ex.Message, "Close");
                }
            }
        }
        
        private async void SettingsBtn_Tapped(object sender, EventArgs e)
        {
            var settingsView = new SettingsViewiOS();
            NavigationPage.SetHasBackButton(settingsView, false);
            await Navigation.PushAsync(settingsView, false);
        }
    }
}
