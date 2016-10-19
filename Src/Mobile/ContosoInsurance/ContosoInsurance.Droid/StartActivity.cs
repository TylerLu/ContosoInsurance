using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Content;

namespace ContosoInsurance.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
             MainLauncher = true,
             NoHistory = true)] 
    public class StartActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        protected override void OnResume()
        {
            base.OnResume();

            Task startupWork = new Task(() =>
            {
                Task.Delay(5000);
            });

            startupWork.ContinueWith(t =>
            {
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }

    }
}