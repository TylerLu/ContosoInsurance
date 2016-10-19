using Xamarin.Forms;
using ContosoInsurance.Views;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Linq;

[assembly: ExportRenderer(typeof(IncidentDescription), typeof(ExtendedPageRenderer))]
[assembly: ExportRenderer(typeof(IncidentDetailView), typeof(ExtendedPageRenderer))]
[assembly: ExportRenderer(typeof(PartyContactView), typeof(ExtendedPageRenderer))]
[assembly: ExportRenderer(typeof(PartyInfoView), typeof(ExtendedPageRenderer))]
[assembly: ExportRenderer(typeof(VehiclesListView), typeof(ExtendedPageRenderer))]
[assembly: ExportRenderer(typeof(SettingsView), typeof(ExtendedPageRendererTwoSide))]
namespace ContosoInsurance.Views
{
    public class ExtendedPageRenderer:PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController.TopViewController.NavigationItem.RightBarButtonItems.Count() > 0)
            {
                var infoButton = NavigationController.TopViewController.NavigationItem.RightBarButtonItems[0];
                NavigationController.TopViewController.NavigationItem.LeftBarButtonItem = infoButton;

                // var favButton = NavigationController.TopViewController.NavigationItem.RightBarButtonItems[1];
                NavigationController.TopViewController.NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { };//new UIBarButtonItem[1] { favButton };
            }

        }
    }

    public class ExtendedPageRendererTwoSide : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController.TopViewController.NavigationItem.RightBarButtonItems.Count() > 0)
            {
                var infoButton = NavigationController.TopViewController.NavigationItem.RightBarButtonItems[0];
                NavigationController.TopViewController.NavigationItem.LeftBarButtonItem = infoButton;

                 var saveButton = NavigationController.TopViewController.NavigationItem.RightBarButtonItems[1];
                if (saveButton != null)
                {
                    NavigationController.TopViewController.NavigationItem.RightBarButtonItems = new UIBarButtonItem[] { saveButton };
                }
                
            }

        }
    }
}
