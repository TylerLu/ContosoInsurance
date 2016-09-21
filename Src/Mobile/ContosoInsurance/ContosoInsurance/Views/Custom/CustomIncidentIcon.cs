using Xamarin.Forms;

namespace ContosoInsurance.Views
{
	public class CustomIncidentIcon : ContentView
    {
        public Image icon { get; set; }
        public StackLayout st { get; set; }

        public int index { get; set; }

        public CustomIncidentIcon (int step)
		{
            st = new StackLayout();
            st.VerticalOptions = LayoutOptions.CenterAndExpand;
            st.Padding = new Thickness(1, 1);

            icon = new Image { BackgroundColor = Color.White, HeightRequest = 39};
            st.Children.Add(icon);
            this.index = step;

            Content = st;
        }
	}
}
