using Corolla_GUIMVVM_E120.ViewModels.Base;
using Corolla_GUIMVVM_E120.Services.SerialDeviceService;

using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;

namespace Corolla_GUIMVVM_E120.ViewModels
{
    public class WarningLightsViewModel : ViewModelBase
    {
        #region Variables
        private readonly ISerialDeviceService _serialDeviceService;

        private bool _enabledWarningLigth = true;
        private bool _chekedWarningLigth = false;

        #endregion

        #region Propiedades

        public bool EnableWarningLigth
        {
            get => _enabledWarningLigth;
            set => SetProperty(ref _enabledWarningLigth, value);
        }
        public bool ChekedWarningLigth
        {
            get => _chekedWarningLigth;
            set => SetProperty(ref _chekedWarningLigth, value);
        }
        #endregion

        #region Comandos
        private DelegateCommand _warningLigth;

        public ICommand WarningLigth => _warningLigth = _warningLigth ?? new DelegateCommand(WarningLigthCommand);
        #endregion

        #region Metodos

        public WarningLightsViewModel(ISerialDeviceService serialDeviceService)
        {
            _serialDeviceService = serialDeviceService;
            _serialDeviceService.EventDeviceStatus += DeviceStatus;
        }

        private void DeviceStatus(object sender, Models.DeviceStatusEventArgs e)
        {
            EnableWarningLigth = e.IsDeviceConnected;
        }

        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            return null;
        }

        private void WarningLigthCommand()
        {
            if (ChekedWarningLigth)
            {
                _serialDeviceService.WriteToPort("<w,1>");
            }
            else
            {
                _serialDeviceService.WriteToPort("<w,0>");
            }
        }
        #endregion
    }
}
