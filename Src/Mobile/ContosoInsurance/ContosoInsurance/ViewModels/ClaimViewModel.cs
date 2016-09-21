using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Files;
using ContosoInsurance.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ContosoInsurance.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ContosoInsurance.ViewModels
{
    public enum ClaimImageTypeModel
    {
        LicensePlate = 1,
        InsuranceCard = 2,
        DriversLicense = 3,
        IncidentImage
    }
    public static class ClaimImage
    {
        public static string IncidentImagePrefix = "claim-";
        public static string getImageKindPrefix(ClaimImageTypeModel type)
        {
            string prefix = string.Empty;
            if (type == ClaimImageTypeModel.DriversLicense)
                prefix = "other-party-license-";
            else if (type == ClaimImageTypeModel.LicensePlate)
                prefix = "other-party-plate-";
            else if (type == ClaimImageTypeModel.InsuranceCard)
                prefix = "other-party-card-";
            else if (type == ClaimImageTypeModel.IncidentImage)
                prefix = IncidentImagePrefix;
            return prefix;
        }
        public static string getPartyImageKindTitle(ClaimImageTypeModel type)
        {
            string title = string.Empty;
            if (type == ClaimImageTypeModel.DriversLicense)
                title = "DRIVERS LICENSE";
            else if (type == ClaimImageTypeModel.LicensePlate)
                title = "LICENSE PLATE";
            else if (type == ClaimImageTypeModel.InsuranceCard)
                title = "INSURANCE CARD";
            return title;
        }
        public static string getPartyImageKindPlaceHolder(ClaimImageTypeModel type)
        {
            string placeHolder = string.Empty;
            if (type == ClaimImageTypeModel.DriversLicense)
                placeHolder = "other-party-license-{Correlation Id}";
            else if (type == ClaimImageTypeModel.LicensePlate)
                placeHolder = "other-party-plate-{Correlation Id}";
            else if (type == ClaimImageTypeModel.InsuranceCard)
                placeHolder = "other-party-card-{Correlation Id}";
            return placeHolder;
        }
    }

    public class ClaimViewModel: BaseViewModel
    {
        private ObservableCollection<FileViewModel> _images;

        public ObservableCollection<FileViewModel> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged(nameof(Images));
            }
        }

        public Claim Claim { get; set; }
        
        public ClaimViewModel(Claim Claim, object parent)
        {
            this.Claim = Claim;
            this.Images = new ObservableCollection<FileViewModel>();
            this.OtherPartyMobilePhone = string.Empty;
            ParentPage = parent;
        }

        private string _otherpartymobilephone;
        public string OtherPartyMobilePhone
        {
            get { return _otherpartymobilephone; }
            set {
                _otherpartymobilephone = value;
                OnPropertyChanged(nameof(_otherpartymobilephone));
            }
        }

        public object ParentPage { get; set; }

        public void PropertyChangeImages()
        {
            OnPropertyChanged(nameof(Images));
        }

        public FileViewModel getIncidentIconFile(int index)
        {
            string end = string.Format("-0{0}", index);
            var fileModel = Images.Where(i => i.File.Name.StartsWith(ClaimImage.IncidentImagePrefix) && i.File.Name.EndsWith(end)).SingleOrDefault();
            return fileModel;
        }
        public FileViewModel getIncidentSelectIconFile()
        {
            var fileModel = Images.Where(i => i.File.Name.StartsWith(ClaimImage.IncidentImagePrefix) && i.Selected).SingleOrDefault();
            return fileModel;
        }
        public int getIncidentSelectIconIndex()
        {
            var fileModel = Images.Where(i => i.File.Name.StartsWith(ClaimImage.IncidentImagePrefix) && i.Selected).SingleOrDefault();
            if(fileModel != null)
            {
                string str = fileModel.File.Name.Substring(fileModel.File.Name.Length - 2);
                int index = Convert.ToInt16(str);
                return index;
            }
            else
            {
                var list = Images.Where(i => i.File.Name.StartsWith(ClaimImage.IncidentImagePrefix));
                if (list != null)
                {
                    return list.Count() + 1;
                }
                else
                {
                    return 1;
                }
            }
        }

        public int getKindImagesFileCount(ClaimImageTypeModel type)
        {
            string prefix = ClaimImage.getImageKindPrefix(type);
            if (prefix.Length >0)
            {
                var fileModel = Images.Where(i => i.File.Name.StartsWith(prefix));
                if (fileModel != null) return fileModel.Count();
            }
            return 0;
        }

        public string[] getPartyImageTitleAndFilenameInfo(ClaimImageTypeModel type)
        {
            string title= ClaimImage.getPartyImageKindTitle(type);
            string placeHolder = ClaimImage.getPartyImageKindPlaceHolder(type);
            string prefix = ClaimImage.getImageKindPrefix(type);
            var files = new List<FileViewModel>();
            if (Images != null && Images.Count > 0 && prefix.Length > 0)
            {
                files = Images.Where(i => i.File.Name.StartsWith(prefix)).ToList();
            }

            if (files.Count > 0)
            {
                placeHolder = files[0].File.Name;
            }
            return new string[] { title,placeHolder};
        }


        public async Task UpdateClaimPartyMobileAsync(string phone)
        {
            OtherPartyMobilePhone = phone;
            Claim.OtherPartyMobilePhone = phone;

            var claimTableAsync = MobileServiceHelper.msInstance.claimTableSync;
            await claimTableAsync.UpdateAsync(Claim);
        }
        public async Task UpdateClaimIncidentDescriptionAsync(string des)
        {
            Claim.Description = des;
            var claimTableAsync = MobileServiceHelper.msInstance.claimTableSync;
            await claimTableAsync.UpdateAsync(Claim);
        }
        public async Task AddNewClaimFileAsync(string fileName, string filePath)
        {
            var claimTableAsync = MobileServiceHelper.msInstance.claimTableSync;

            FileViewModel fileModel = new FileViewModel
            {
                File = await claimTableAsync.AddFileAsync(Claim, fileName),
                Uri = filePath,
                Selected = true
            };
            Images.Add(fileModel);
            return;
        }
        public async Task PushClaimFileChangesAsync(Claim cl)
        {
            var client = MobileServiceHelper.msInstance.Client;
            var claimTableAsync = MobileServiceHelper.msInstance.claimTableSync;

            await claimTableAsync.PushFileChangesAsync();
            await client.SyncContext.PushAsync();
            
            var jsonRequest = new JObject();
            jsonRequest["Id"] = cl.Id;

            await client.InvokeApiAsync(string.Format("SubmitClaimForProcessing/{0}", cl.Id), jsonRequest, HttpMethod.Post, null);
            return;

        }
    }
}
