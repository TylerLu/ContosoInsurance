using ContosoInsurance.Views;
using Xamarin.Forms;
using ContosoInsurance.Helpers;

namespace ContosoInsurance
{
    public class App : Application
    {
        public const string AppName = "ContosoInsurance";
        public static App Instance;
        public static object UIContext { get; set; }

        public App()
        {
            Instance = this;
            MobileServiceHelper.msInstance = new MobileServiceHelper();

            var loginView = new LoginView();
            MainPage = new NavigationPage(loginView);
            NavigationPage.SetHasNavigationBar(loginView, false);
        }
    }
}
