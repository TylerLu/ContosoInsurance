using System;
using Xamarin.Forms;
using ContosoInsurance.Views;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[assembly: ExportRenderer(typeof(CustomNumberEntry), typeof(CustomNumberEntryRenderer))]
namespace ContosoInsurance.Views
{
    public class CustomNumberEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
            }
        }
    }
}