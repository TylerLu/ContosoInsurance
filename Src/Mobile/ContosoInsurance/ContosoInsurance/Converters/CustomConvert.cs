using System;
using Xamarin.Forms;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ContosoInsurance.ViewModels;

namespace ContosoInsurance.Converters
{
    class RevertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PartyImageSourceConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ClaimImageTypeModel type = (ClaimImageTypeModel)parameter;
            var Images = (ObservableCollection<FileViewModel>)value;
            string prefix = ClaimImage.getImageKindPrefix(type);
            var ret = new List<FileViewModel>();

            if (prefix.Length > 0 && Images != null && Images.Count > 0)
            {
                ret = Images.Where(i => i.File.Name.StartsWith(prefix)).ToList();
            }
            if (ret.Count > 0)
            {
                return ret[0].Uri;
            }
            else
            {
                return "CameraBk.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PartyCameraHasImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ClaimImageTypeModel type = (ClaimImageTypeModel)parameter;
            var Images = (ObservableCollection<FileViewModel>)value;
            string prefix = ClaimImage.getImageKindPrefix(type);
            var ret = new List<FileViewModel>();

            if (prefix.Length >0 && Images != null && Images.Count > 0)
            {
                ret = Images.Where(i => i.File.Name.StartsWith(prefix)).ToList();
            }
            if (ret.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class IncidentIconBkConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Images = (ObservableCollection<FileViewModel>)value;

            int index = (int)parameter;
            
            string start= ClaimImage.IncidentImagePrefix;
            string end = string.Format("-0{0}", index);
            var file = new List<FileViewModel>();

            if (Images != null && Images.Count > 0)
            {
                var fileModel = Images.Where(i => i.File.Name.StartsWith(start) && i.File.Name.EndsWith(end)).SingleOrDefault();
                if(fileModel != null)
                {
                    return fileModel.BackGroundColor;
                }
            }
            if(Images.Where(i => i.File.Name.StartsWith(start) && i.Selected).Count() == 0)
            {
                return Color.FromHex("0092ff");
            }
            else
            {
                return Color.White;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class IncidentIconIsVisibleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Images = (ObservableCollection<FileViewModel>)value;

            int index = (int)parameter;

            string start = ClaimImage.IncidentImagePrefix;
            string end = string.Format("-0{0}", index);

            if (Images != null && Images.Count > 0)
            {
                var fileModel = Images.Where(i => i.File.Name.StartsWith(start) && i.File.Name.EndsWith(end)).SingleOrDefault();
                if (fileModel != null)
                {
                    return true;
                }
            }
            if (index == 1) return true;
            else
            {
                var files = Images.Where(i => i.File.Name.StartsWith(start));
                if (files != null && files.Count()+1 == index)
                    return true;
                else
                {
                    return false;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class IncidentIconSourceConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = (int)parameter;
            var Images = (ObservableCollection<FileViewModel>)value;

            string start = ClaimImage.IncidentImagePrefix;
            string end = string.Format("-0{0}", index);
            if (Images != null && Images.Count > 0)
            {
                var fileModel = Images.Where(i => i.File.Name.StartsWith(start) && i.File.Name.EndsWith(end)).SingleOrDefault();
                if (fileModel != null)
                {
                    return fileModel.Uri;
                }
            }
            return "AddNewImg.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class IncidentSelectImageSourceConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Images = (ObservableCollection<FileViewModel>)value;

            string start = ClaimImage.IncidentImagePrefix;
            if (Images != null && Images.Count > 0)
            {
                var fileModel = Images.Where(i => i.File.Name.StartsWith(start) && i.Selected).SingleOrDefault();
                if (fileModel != null)
                {
                    return fileModel.Uri;
                }
            }
            return "CameraBk.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    class IncidentTakeNewButtonIsVisbleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var Images = (ObservableCollection<FileViewModel>)value;

            string start = ClaimImage.IncidentImagePrefix;
            if (Images != null && Images.Count > 0)
            {
                var fileModel = Images.Where(i => i.File.Name.StartsWith(start) && i.Selected).SingleOrDefault();
                if (fileModel != null)
                {
                    return true;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
