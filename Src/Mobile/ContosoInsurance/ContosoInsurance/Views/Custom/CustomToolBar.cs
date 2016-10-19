using Xamarin.Forms;

namespace ContosoInsurance.Views
{
    public class CustomToolBar : ContentView
    {
        public View PreviousButton { get; set; }
        public View NextButton { get; set; }
        public CustomToolBar()
        {
            var isIOS = Device.OS == TargetPlatform.iOS;
            var buttonWidth = isIOS ? 13 : 50;

            var bottomGrid = new Grid();
            bottomGrid.RowSpacing = 0;
            bottomGrid.ColumnSpacing = 0;

            bottomGrid.BackgroundColor = Color.FromHex(isIOS ? "F9F9F9" : "D6D6D6");
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(buttonWidth) });
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            bottomGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(buttonWidth) });

            if (isIOS)
            {
                PreviousButton = new Image
                {
                    Source = "back.png",
                    Aspect = Aspect.AspectFit
                };
            }
            else
            {
                PreviousButton = new Label
                {
                    Text = "BACK",
                    FontSize = 16,
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Color.Black
                };
            }
            PreviousButton.VerticalOptions = LayoutOptions.CenterAndExpand;
            PreviousButton.HorizontalOptions = LayoutOptions.FillAndExpand;
            PreviousButton.IsVisible = false;

            if (isIOS)
            {
                NextButton = new Image
                {
                    Source = "forward.png",
                    Aspect = Aspect.AspectFit
                };
            }
            else
            {
                NextButton = new Label
                {
                    Text = "NEXT",
                    FontSize = 16,
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Color.Black
                };
            }
            NextButton.VerticalOptions = LayoutOptions.CenterAndExpand;
            NextButton.HorizontalOptions = LayoutOptions.FillAndExpand;

            bottomGrid.Children.Add(PreviousButton, 0, 0);
            bottomGrid.Children.Add(NextButton, 2, 0);
            bottomGrid.Padding = new Thickness(15, 0);
            Content = bottomGrid;
        }
    }
}
