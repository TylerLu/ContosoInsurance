
using Xamarin.Forms;
using ContosoInsurance.Models;

namespace ContosoInsurance.Views
{
	public class CustomVehicleFrame : Frame
	{
        public int VehicleId { get; set; }
        public CustomVehicleFrame(Vehicle ve)
		{
            VehicleId = ve.VehicleId;
            Padding = new Thickness(2, 2);
            Image image = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            StackLayout imageStack = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { image }
            };

            Label label = new Label
            {
                Text = ve.LicensePlate,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
            };

            StackLayout StackLayout1 = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children ={ imageStack, label }
            };

            Content = StackLayout1;
            HasShadow = false;
            OutlineColor = Color.FromHex("d2d2d2");
            BindingContext = ve;
            image.SetBinding(Image.SourceProperty, Binding.Create((Vehicle vm) => vm.Uri, BindingMode.OneWay));
            SetBinding(BackgroundColorProperty, Binding.Create((Vehicle vm) => vm.BackGroundColor, BindingMode.OneWay));
            label.SetBinding(Label.TextColorProperty, Binding.Create((Vehicle vm) => vm.TextColor, BindingMode.OneWay));
            image.HeightRequest = 54;
            imageStack.HeightRequest = 70;
            label.HeightRequest = 35;
            HeightRequest = 105;
            Device.OnPlatform(
                    iOS: () =>
                    {
                        image.HeightRequest = 54;
                        imageStack.HeightRequest = 70;
                        label.HeightRequest = 35;
                        HeightRequest = 105;
                    },
                    Android: () =>
                    {
                        image.HeightRequest = 64;
                        imageStack.HeightRequest = 84;
                        label.HeightRequest = 42;
                        HeightRequest = 126;
                    });
        }
    }
}
