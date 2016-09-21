using System;
using System.Diagnostics;
using Xamarin.Forms;
using ContosoInsurance.ViewModels;
using ContosoInsurance.Converters;
using ContosoInsurance.Helpers;

namespace ContosoInsurance.Views
{
	public partial class IncidentDetailViewiOS : ContentPage
	{
        private ClaimViewModel claimViewModel;
        public IncidentDetailViewiOS (ClaimViewModel cl)
		{
            Title = "Contoso Insurance";
            InitializeComponent();
            claimViewModel = cl;
            BindingContext = claimViewModel;

            AddIncidentIcon();
            InitBottomToolBar();
            cameraLabel.SetBinding(Label.IsVisibleProperty, 
                new Binding { Source = BindingContext, Path = "Images", Converter = new IncidentTakeNewButtonIsVisbleConvert(), Mode = BindingMode.OneWay });

        }

        public void InitBottomToolBar()
        {
            var nextTapGestureRecognizer = new TapGestureRecognizer();
            nextTapGestureRecognizer.Tapped += NextButton_Clicked;
            bottomView.NextImage.GestureRecognizers.Add(nextTapGestureRecognizer);

            bottomView.PreviousImage.IsVisible = true;
            var backTapGestureRecognizer = new TapGestureRecognizer();
            backTapGestureRecognizer.Tapped += PreviousButton_Clicked;
            bottomView.PreviousImage.GestureRecognizers.Add(backTapGestureRecognizer);
        }

        public void AddIncidentIcon()
        {
            for(int i = 1; i< 6 ; i++)
            {
                CustomIncidentIcon incidentIcon = new CustomIncidentIcon(i);

                var iconTapGestureRecognizer = new TapGestureRecognizer();
                iconTapGestureRecognizer.Tapped +=IncidentIcon_Tapped;
                incidentIcon.st.GestureRecognizers.Add(iconTapGestureRecognizer);

                incidentIcon.st.SetBinding(StackLayout.BackgroundColorProperty,
                    new Binding { Source = BindingContext, Path = "Images", Converter = new IncidentIconBkConvert(), Mode = BindingMode.OneWay, ConverterParameter = i });
                incidentIcon.st.SetBinding(StackLayout.IsVisibleProperty,
                    new Binding { Source = BindingContext, Path = "Images", Converter = new IncidentIconIsVisibleConvert(), Mode = BindingMode.OneWay, ConverterParameter = i });
                incidentIcon.icon.SetBinding(Image.SourceProperty,
                    new Binding { Source = BindingContext, Path = "Images", Converter = new IncidentIconSourceConvert(), Mode = BindingMode.OneWay, ConverterParameter = i });
                this.incidentIconsCtrl.Children.Add(incidentIcon, i-1, 0);
            }
            this.selectImageSource.SetBinding(Image.SourceProperty,
                    new Binding { Source = BindingContext, Path = "Images", Converter = new IncidentSelectImageSourceConvert(), Mode = BindingMode.OneWay });
        }

        public async void IncidentIcon_Tapped(object sender, EventArgs e)
        {
            CustomIncidentIcon icon = (CustomIncidentIcon)((Element)sender).Parent;
            var fileModel = claimViewModel.getIncidentIconFile(icon.index);
            var selectFileModel = claimViewModel.getIncidentSelectIconFile();
            if (selectFileModel != null)
            {
                selectFileModel.Selected = false;
            }
            if (fileModel != null)
            {
                fileModel.Selected = true;
            }
            claimViewModel.PropertyChangeImages(); ;
        }

        public async void CameraBtn_Tapped(object sender, EventArgs e)
        {
            using (var scope = new ActivityIndicatorScope(activityIndicator, activityIndicatorPanel, true))
            {
                try
                {
                    int selectIndex = claimViewModel.getIncidentSelectIconIndex();

                    IPlatform platform = DependencyService.Get<IPlatform>();
                    string sourceImagePath = await platform.TakePhotoAsync(App.UIContext);
                    if (sourceImagePath != null)
                    {

                        string fileName = "claim-{Correlation Id}-0" + selectIndex.ToString();
                        fileName = fileName.Replace("{Correlation Id}", this.claimViewModel.Claim.Id);
                        string copiedFilePath = await FileHelper.CopyFileAsync(this.claimViewModel.Claim.Id, fileName, sourceImagePath, MobileServiceHelper.msInstance.DataFilesPath);

                        var fileModel = claimViewModel.getIncidentIconFile(selectIndex);
                        if (fileModel == null)
                        {
                            await claimViewModel.AddNewClaimFileAsync(fileName, copiedFilePath);
                        }
                        //refresh
                        claimViewModel.PropertyChangeImages(); ;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Image upload failed." + ex.Message);
                    Trace.WriteLine("Image upload failed - " + ex);
                    await DisplayAlert("Image upload failed", "Image upload failed. Please try again later", "Ok");
                }
            }
        }

        #region User Common Actions

        private async void PreviousButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync(true);
            return;
        }

        private async void NextButton_Clicked(object sender, EventArgs e)
        {
            if (claimViewModel.getKindImagesFileCount(ClaimImageTypeModel.IncidentImage) > 0)
            {
                var nextPage = new IncidentDescriptioniOS(claimViewModel);
                await Navigation.PushAsync(nextPage, true);
                NavigationPage.SetHasBackButton(nextPage, false);
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
