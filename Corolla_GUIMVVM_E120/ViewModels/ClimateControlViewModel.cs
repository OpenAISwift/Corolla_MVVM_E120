using Corolla_GUIMVVM_E120.ViewModels.Base;
using Corolla_GUIMVVM_E120.Services.SerialDeviceService;
using Corolla_GUIMVVM_E120.Models;

using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Corolla_GUIMVVM_E120.ViewModels
{
    public class ClimateControlViewModel : ViewModelPageBase
    {
        private readonly ISerialDeviceService _serialDeviceService;

        public DeviceModel Device = new DeviceModel();


        public ClimateControlViewModel(ISerialDeviceService serialDeviceService)
        {
            _serialDeviceService = serialDeviceService;

            _serialDeviceService.InitDeviceAsync();

            Device.Name = "1";
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
