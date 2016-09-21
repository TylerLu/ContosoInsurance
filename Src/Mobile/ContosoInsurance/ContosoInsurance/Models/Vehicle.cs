using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices;
using ContosoInsurance.Helpers;
using Xamarin.Forms;

namespace ContosoInsurance.Models
{
    public class Vehicle: INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string LicensePlate { get; set; }
        public string VIN { get; set; }

        public int VehicleId { get; set; }

        [Version]
        public string Version { get; set; }

        private string _uri;
        private MobileServiceFile _file;
        private bool _imageLoaded;

        [JsonIgnore]
        public string Uri
        {
            get
            {
                return ImageLoaded ? _uri : "";
            }

            set
            {
                _uri = value;
                OnPropertyChanged(nameof(Uri));
            }
        }
        [JsonIgnore]
        public MobileServiceFile File
        {
            get { return _file; }
            set
            {
                _file = value;

                if (_file != null)
                {
                    FileHelper.GetLocalFilePathAsync(Id, _file.Name, MobileServiceHelper.msInstance.DataFilesPath)
                        .ContinueWith(x => this.Uri = x.Result);
                }

                OnPropertyChanged(nameof(File));
            }
        }

        [JsonIgnore]
        public bool ImageLoaded
        {
            get { return _imageLoaded; }
            set
            {
                _imageLoaded = value;
                OnPropertyChanged(nameof(ImageLoaded));
                OnPropertyChanged(nameof(Uri));
            }
        }


        private Boolean _selected { get; set; }
        [JsonIgnore]
        public Boolean Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged(nameof(BackGroundColor));
                OnPropertyChanged(nameof(TextColor));
            }
        }
        [JsonIgnore]
        public Color BackGroundColor
        {
            get
            {
                if (Selected)
                {
                    return Color.FromHex("276bfa");
                }
                else
                {
                    return Color.FromHex("e8e8e8");
                }
            }
        }
        [JsonIgnore]
        public Color TextColor
        {
            get
            {
                if (Selected)
                {
                    return Color.FromHex("e3e8e4");
                }
                else
                {
                    return Color.FromHex("3e4854");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
