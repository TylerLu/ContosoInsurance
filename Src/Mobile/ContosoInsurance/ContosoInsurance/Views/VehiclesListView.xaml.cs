using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Linq;
using ContosoInsurance.Helpers;
using Microsoft.WindowsAzure.MobileServices.Sync;
using PCLStorage;
using ContosoInsurance.Models;
using ContosoInsurance.ViewModels;

namespace ContosoInsurance.Views
{
	public partial class VehiclesListView : ContentPage
    {
        private VehiclesListViewModel viewModel;
        public VehiclesListView()
        {
            this.Title = "Contoso Insurance";
            InitializeComponent();

            viewModel = new VehiclesListViewModel();
            BindingContext = viewModel;
            InitBottomToolBar();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.vehiclesData.Count == 0)
            {
                try
                {
                    using (var scope = new ActivityIndicatorScope(activityIndicator, activityIndicatorPanel, true))
                    {
                        var vehicles = await viewModel.GetVehiclesAsync();
                        foreach (var ve in vehicles)
                        {
                            var result = await viewModel.GetVehicleFileAsync(ve);
                            ve.File = result.FirstOrDefault();
                            if (ve.File != null)
                            {
                                string filePath = await FileHelper.GetLocalFilePathAsync(ve.Id, ve.File.Name, MobileServiceHelper.msInstance.DataFilesPath);
                                ve.ImageLoaded = await FileSystem.Current.LocalStorage.CheckExistsAsync(filePath) == ExistenceCheckResult.FileExists;
                            }
                            viewModel.vehiclesData.Add(ve);
                            ve.Selected = false;

                            CustomVehicleFrame frame = new CustomVehicleFrame(ve);
                            var tgr = new TapGestureRecognizer();
                            tgr.Tapped += MyVehicle_Tapped;
                            frame.GestureRecognizers.Add(tgr);
                            vehicleListCtrl.Children.Add(frame);
                        }
                        vehicleListCtrl.Padding = ((Device.OS == TargetPlatform.iOS) ? new Thickness(0, 0, 0, 20): new Thickness(0, 0, 0, 30));
                    } 
                }
                catch (Exception ex)
                {
                    Utils.TraceException("Failed to get vehicles - ", ex);
                    await DisplayAlert("Error", "Failed to get vehicles - " + ex.Message, "Close");
                    await Navigation.PopToRootAsync();
                }
            }
        }

        public void InitBottomToolBar()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += NextButton_Clicked;
            bottomView.NextButton.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void EmptyClaimViewModel()
        {
            viewModel.claimViewModel = null;
            viewModel.bCreateNewClaim = true;
        }
        
        private void MyVehicle_Tapped(object sender, EventArgs e)
        {
            CustomVehicleFrame button = (CustomVehicleFrame)sender;
            Vehicle buttonModel = viewModel.vehiclesData.First(m => m.VehicleId.Equals(button.VehicleId));
            if (!buttonModel.Selected)
            {
                Vehicle previousButtonModel = viewModel.vehiclesData.FirstOrDefault(m => m.Selected.Equals(true));
                if (previousButtonModel != null)
                {
                    previousButtonModel.Selected = false;
                }
                viewModel.bCreateNewClaim = true;
                viewModel.selectVehicleId = buttonModel.VehicleId;
                buttonModel.Selected = true;
            }
        }

        #region User Common Actions

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            if (viewModel.selectVehicleId == null) return;
            using (var scope = new ActivityIndicatorScope(activityIndicator, activityIndicatorPanel, true))
            {
                await viewModel.CheckCreateClaimAsync(this);
                var partyView = new PartyInfoView(viewModel.claimViewModel, ClaimImageTypeModel.LicensePlate);
                NavigationPage.SetHasBackButton(partyView, false);
                await Navigation.PushAsync(partyView, true);
            }
        }

        public async void MenuClicked(object sender, EventArgs e)
        {
            this.menuList.IsVisible = !this.menuList.IsVisible;
        }

        public async void LogoutBtn_Tapped(object sender, EventArgs e)
        {
            this.menuList.IsVisible = false;
            await MobileServiceHelper.msInstance.DoLogOutAsync();
            await Navigation.PopToRootAsync(true);
        }

        public async void SettingsBtn_Tapped(object sender, EventArgs e)
        {
            this.menuList.IsVisible = false;
            var settingsView = new SettingsView();
            NavigationPage.SetHasBackButton(settingsView, false);
            await Navigation.PushAsync(settingsView, false);
        }

        #endregion
    }
}
