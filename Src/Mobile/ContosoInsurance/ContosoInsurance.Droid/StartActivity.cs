using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;

namespace ContosoInsurance.Droid
{
    [Activity(Theme = "@style/Theme.Splash",
             MainLauncher = true,
             NoHistory = true)] 
    public class StartActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

    }
}