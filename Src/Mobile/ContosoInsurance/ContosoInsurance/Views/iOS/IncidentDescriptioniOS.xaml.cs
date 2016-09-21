using System;
using System.Diagnostics;
using Xamarin.Forms;
using ContosoInsurance.ViewModels;
using ContosoInsurance.Helpers;


namespace ContosoInsurance.Views
{
	public partial class IncidentDescriptioniOS : ContentPage
	{
        private ClaimViewModel claimViewModel;
        public IncidentDescriptioniOS (ClaimViewModel cl)
		{
            Title = "Contoso Insurance";
            InitializeComponent();
            claimViewModel = cl;
            InitBottomToolBar();
            this.incidentDescription.Text = cl.Claim.Description;
            this.incidentDescription.Completed += IncidentDescription_Completed;
        }

        private void IncidentDescription_Completed(object sender, EventArgs e)
        {
           claimViewModel.Claim.Description = ((Editor)sender).Text;
        }

        private void InitBottomToolBar()
        {
            var nextTapGestureRecognizer = new TapGestureRecognizer();
            nextTapGestureRecognizer.Tapped += NextButton_Clicked;
            bottomView.NextImage.GestureRecognizers.Add(nextTapGestureRecognizer);

            bottomView.PreviousImage.IsVisible = true;
            var backTapGestureRecognizer = new TapGestureRecognizer();
            backTapGestureRecognizer.Tapped += PreviousButton_Clicked;
            bottomView.PreviousImage.GestureRecognizers.Add(backTapGestureRecognizer);
        }

        #region User Common Actions

        private async void PreviousButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
            return;
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                using (var scope = new ActivityIndicatorScope(activityIndicator, activityIndicatorPanel, true))
                {
                    await claimViewModel.UpdateClaimIncidentDescriptionAsync(this.incidentDescription.Text);
                    await claimViewModel.PushClaimFileChangesAsync(claimViewModel.Claim);
                    await DisplayAlert("Thank you.", "Your claim has been submitted.", "Close");

                    ((VehiclesListViewiOS)claimViewModel.ParentPage).EmptyClaimViewModel();
                    for (int i = Navigation.NavigationStack.Count - 1; i > 1; i--)
                    {
                        Page removedPage = Navigation.NavigationStack[i];
                        Navigation.RemovePage(removedPage);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to submit claim - " + ex.Message);
                Trace.WriteLine("Failed to submit claim - " + ex);
                await DisplayAlert("Error", ex.Message, "Close");
            }
        }

        private async void MenuClicked(object sender, EventArgs e)
        {
            this.menuList.IsVisible = !this.menuList.IsVisible;
        }

        private async void LogoutBtn_Tapped(object sender, EventArgs e)
        {
            this.menuList.IsVisible = false;
            await MobileServiceHelper.msInstance.DoLogOutAsync();
            await Navigation.PopToRootAsync(true);
        }

        private async void SettingsBtn_Tapped(object sender, EventArgs e)
        {
            this.menuList.IsVisible = false;
            var settingsView = new SettingsViewiOS();
            NavigationPage.SetHasBackButton(settingsView, false);
            await Navigation.PushAsync(settingsView, false);
        }

        #endregion
    }
}
