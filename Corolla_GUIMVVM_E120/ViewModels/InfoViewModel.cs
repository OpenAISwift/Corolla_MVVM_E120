using Corolla_GUIMVVM_E120.Models;
using Corolla_GUIMVVM_E120.Services.SerialDeviceService;
using Corolla_GUIMVVM_E120.ViewModels.Base;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

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
            _serialDeviceService.EventDeviceNewData += EventDeviceNewData; ;
        }

        private void EventDeviceNewData(object sender, DeviceNewDataEventArgs e)
        {
            AmbientTemperature = e.AmbientTemperature;
        }

        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            return null;
        }

    }
}
