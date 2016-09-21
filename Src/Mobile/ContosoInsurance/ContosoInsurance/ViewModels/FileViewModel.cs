using System;
using Microsoft.WindowsAzure.MobileServices.Files;
using Xamarin.Forms;

namespace ContosoInsurance.ViewModels
{
    public class FileViewModel: BaseViewModel
    {
        private MobileServiceFile _file;
        private string _uri;
        public MobileServiceFile File
        {
            get { return _file; }
            set
            {
                _file = value;
                OnPropertyChanged(nameof(Uri));
            }
        }

        public string Uri
        {
            get
            {
                return _uri;
            }

            set
            {
                _uri = value;
                OnPropertyChanged(nameof(Uri));
            }
        }

        private Boolean _selected { get; set; }
        public Boolean Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(BackGroundColor));
            }
        }
        public Color BackGroundColor
        {
            get
            {
                if (Selected)
                {
                    return Color.FromHex("0092ff");
                }
                else
                {
                    return Color.White;
                }
            }
        }
    }
}
