using Xamarin.Forms;

namespace ContosoInsurance.Views
{
	public class CustomToolBariOS : ContentView
	{
        public Image PreviousImage { get; set; }
        public Image NextImage { get; set; }
        public CustomToolBariOS()
        {
            var bottomGrid = new Grid();
            bottomGrid.RowSpacing = 0;
            bottomGrid.ColumnSpacing = 0;
            
            bottomGrid.BackgroundColor = Color.FromHex("F9F9F9");
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(13) });
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star});
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(13) });

            PreviousImage = new Image
            {
                Source = "back.png",
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            PreviousImage.IsVisible = false;
            bottomGrid.Children.Add(PreviousImage, 0, 0);

            NextImage = new Image
            {
                Source = "forward.png",
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            bottomGrid.Padding = new Thickness(15, 0);
            bottomGrid.Children.Add(NextImage, 2, 0);

            Content = bottomGrid;
        }
    }
}
