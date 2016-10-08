using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PCLStorage;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices.Files;
using Microsoft.WindowsAzure.MobileServices.Sync;
using ContosoInsurance.Models;
using ContosoInsurance.Helpers;
using Xamarin.Geolocation;
using Xamarin.Forms;

namespace ContosoInsurance.ViewModels
{
    public class VehiclesListViewModel: BaseViewModel
    {
        private int? _selectVehicleId;
        public ObservableCollection<Vehicle> vehiclesData { get; set;}

        public int? selectVehicleId
        {
            get
            {
                return _selectVehicleId;
            }

            set
            {
                _selectVehicleId = value;
                OnPropertyChanged(nameof(selectVehicleId));
            }
        }
        public bool bCreateNewClaim { get; set; }

        public ClaimViewModel claimViewModel { get; set; }

        private IDisposable eventSubscription;

        public VehiclesListViewModel()
        {
            vehiclesData = new ObservableCollection<Vehicle>();
            bCreateNewClaim = true;
            selectVehicleId = null;

            eventSubscription = MobileServiceHelper.msInstance.Client.EventManager.Subscribe<ImageDownloadEvent>(DownloadStatusObserver);
        }

        private async void DownloadStatusObserver(ImageDownloadEvent evt)
        {
            try
            {
                var ve = vehiclesData.Where(x => x.Id == evt.Id).FirstOrDefault();
                Debug.WriteLine($"Image download event: {ve?.Id}");
                if (ve != null && !ve.ImageLoaded)
                {
                    var result = await GetVehicleFileAsync(ve);
                    ve.File = result.FirstOrDefault();
                    if (ve.File != null)
                    {
                        string filePath = await FileHelper.GetLocalFilePathAsync(ve.Id, ve.File.Name, MobileServiceHelper.msInstance.DataFilesPath);
                        ve.ImageLoaded = await FileSystem.Current.LocalStorage.CheckExistsAsync(filePath) == ExistenceCheckResult.FileExists;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Failed to download image - " + ex.Message);
                Trace.WriteLine("Failed to download image - " + ex);
            }
        }

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            await MobileServiceHelper.msInstance.SyncAsync();

            var veTableSync = MobileServiceHelper.msInstance.vehicleTableSync;
            return await veTableSync.ToListAsync();
        }

        public async Task<IEnumerable<MobileServiceFile>> GetVehicleFileAsync(Vehicle ve)
        {
            var veTableSync = MobileServiceHelper.msInstance.vehicleTableSync;
            return await veTableSync.GetFilesAsync(ve);
        }

        public async Task CheckCreateClaimAsync(object parent)
        {
            if (bCreateNewClaim || claimViewModel == null)
            {
                bCreateNewClaim = false;
                Position pt = await GetGPS();
                Claim claim = new Claim
                {
                    Id = Guid.NewGuid().ToString(),
                    VehicleId = (int)selectVehicleId,
                    Longitude = pt.Longitude,
                    Latitude = pt.Latitude,
                    DateTime = DateTime.Now,
                };
                //create new Claim
                var claimTableSync = MobileServiceHelper.msInstance.claimTableSync;
                await claimTableSync.InsertAsync(claim);

                claimViewModel = new ClaimViewModel(claim, parent);
            }
            return;
        }

        private async Task<Position> GetGPS()
        {
            Position position = new Position();
            try
            {
                IPlatform platform = DependencyService.Get<IPlatform>();
                position = await platform.GetGeolocator(App.UIContext);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failure to get GPS coordinates - " + ex.Message);
                Trace.WriteLine("Failure to get GPS coordinates - " + ex);
            }
            return position;
        }

        public void Dispose()
        {
            eventSubscription.Dispose();
        }
    }
}
