using System;
using System.Diagnostics;
using Xamarin.Forms;
using ContosoInsurance.ViewModels;
using ContosoInsurance.Helpers;


namespace ContosoInsurance.Views
{
	public partial class IncidentDescription : ContentPage
	{
        private ClaimViewModel claimViewModel;
        public IncidentDescription (ClaimViewModel cl)
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
            bottomView.NextButton.GestureRecognizers.Add(nextTapGestureRecognizer);

            bottomView.PreviousButton.IsVisible = true;
            var backTapGestureRecognizer = new TapGestureRecognizer();
            backTapGestureRecognizer.Tapped += PreviousButton_Clicked;
            bottomView.PreviousButton.GestureRecognizers.Add(backTapGestureRecognizer);
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

                    ((VehiclesListView)claimViewModel.ParentPage).EmptyClaimViewModel();

                    //pop up to vehicles list view
                    if (Navigation.NavigationStack.Count > 2)
                    {
                        for (int i = Navigation.NavigationStack.Count - 2; i > 1; i--)
                        {
                            Page removedPage = Navigation.NavigationStack[i];
                            Navigation.RemovePage(removedPage);
                        }
                        Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.TraceException("Failed to submit claim. ", ex);
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
            var settingsView = new SettingsView();
            NavigationPage.SetHasBackButton(settingsView, false);
            await Navigation.PushAsync(settingsView, false);
        }

        #endregion
    }
}
