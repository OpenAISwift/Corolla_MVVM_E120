using Corolla_GUIMVVM_E120.ViewModels.Base;
using Corolla_GUIMVVM_E120.Services.SerialDeviceService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Corolla_GUIMVVM_E120.Models;

namespace Corolla_GUIMVVM_E120.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        ISerialDeviceService _serialDeviceService;

        private string _ambientTemperature = "0";

        public string AmbientTemperature
        {
            get => _ambientTemperature;
            set => SetProperty(ref _ambientTemperature, value);
        }

        public InfoViewModel(ISerialDeviceService serialDeviceService)
        {
            _serialDeviceService = serialDeviceService;
            _serialDeviceService.AvailableData += SerialDeviceAvailableData;
        }

        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {   
            return null;
        }

        private void SerialDeviceAvailableData(object sender, EventArgs e)
        {
            ClimateControlModel climateControl = (ClimateControlModel)sender;
            UpdateViewClimateControl(climateControl);
        }

        private void UpdateViewClimateControl(ClimateControlModel climateControlModel)
        {
            AmbientTemperature = climateControlModel.AmbientTemperature;
        }
    }
}
