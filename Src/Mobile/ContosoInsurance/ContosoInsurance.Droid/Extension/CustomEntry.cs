using System;
using Xamarin.Forms;
using ContosoInsurance.Views;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace ContosoInsurance.Views
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
            }
        }
    }
}