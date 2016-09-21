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
	public partial class VehiclesListViewiOS : ContentPage
    {
        private VehiclesListViewModel viewModel;
        public VehiclesListViewiOS()
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
                using (var scope = new ActivityIndicatorScope(activityIndicator, activityIndicatorPanel, true))
                {
                    try
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
                        vehicleListCtrl.Padding = new Thickness(0, 0, 0, 20);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to get vehicles - " + ex.Message);
                        Trace.WriteLine("Failure to get vehicles - " + ex);
                        await DisplayAlert("Error", "Failed to get vehicles - " + ex.Message, "Close");
                        await Navigation.PopToRootAsync();
                    }
                }
            }
        }

        public void InitBottomToolBar()
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += NextButton_Clicked;
            bottomView.NextImage.GestureRecognizers.Add(tapGestureRecognizer);
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
                var partyView = new PartyInfoViewiOS(viewModel.claimViewModel, ClaimImageTypeModel.LicensePlate);
                await Navigation.PushAsync(partyView, true);
                NavigationPage.SetHasBackButton(partyView, false);
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
            var settingsView = new SettingsViewiOS();
            NavigationPage.SetHasBackButton(settingsView, false);
            await Navigation.PushAsync(settingsView, false);
        }

        #endregion
    }
}
