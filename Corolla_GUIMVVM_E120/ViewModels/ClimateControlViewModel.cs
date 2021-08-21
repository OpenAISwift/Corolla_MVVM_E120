using Corolla_GUIMVVM_E120.ViewModels.Base;
using Corolla_GUIMVVM_E120.Models;

using Corolla_GUIMVVM_E120.Services.SerialDeviceService;
using Corolla_GUIMVVM_E120.Services.LocalAppDataService;
using Corolla_GUIMVVM_E120.Services.DialogService;
using Corolla_GUIMVVM_E120.Services.LoaderService;

using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Corolla_GUIMVVM_E120.Contants;
using System.Windows.Input;
using System;

namespace Corolla_GUIMVVM_E120.ViewModels
{
    public class ClimateControlViewModel : ViewModelPageBase
    {
        #region Variables
        private readonly DeviceModel device;

        private readonly ISerialDeviceService _serialDeviceService;
        private readonly ILocalAppDataService _localAppDataService;
        private readonly IDialogService _dialogService;
        private readonly ILoaderService _loaderService;

        private const char char_StartMarker = '<';
        private const char char_EndMarker = '>';
        private readonly string[] str_Delimiter = new string[] { " ", ",", ":" };

        private string _deviceData = null;
        public string[] str_data;
        private string _ambientTemperature = "0";
        private string _insideTemperature = "0";
        private string _insideHumidity = "0";
        private string _evaporatorTemperature = "0";
        private string _ambientLight = "0";
        private string _dewPointValue = "0";
        private string _thermalSensation = "0";

        private byte value_Blower = 0;
        private byte value_AirMix = 0;

        private bool _enabledFan1 = false;
        private bool _enableDefrosterFront = true;
        private bool _enableDefrostRear = true;
        private bool _enableAc = false;
        private bool _enableAuto = true;
        private bool _enableAirMode = true;
        private bool _enableAirFront = true;
        private bool _enableAirDown = true;
        private bool _enableVentFront = true;
        private bool _enableBlowerUp = true;
        private bool _enableBlowerDown = true;
        private bool _enableAirMixUp = true;
        private bool _enableAirMixDown = true;

        private bool _illumination = false;

        private bool _chekedFan1 = false;
        private bool _chekedDefrosterFront = false;
        private bool _chekedDefrostRear = false;
        private bool _chekedAc = false;
        private bool _chekedAuto = false;
        private bool _chekedAirMode = false;
        private bool _chekedAirFront = false;
        private bool _chekedAirDown = false;
        private bool _chekedVentFront = false;

        private bool _blowerStatus0 = true;
        private bool _blowerStatus1 = false;
        private bool _blowerStatus2 = false;
        private bool _blowerStatus3 = false;
        private bool _blowerStatus4 = false;
        private bool _blowerStatus5 = false;
        private bool _blowerStatus6 = false;

        private bool _airMixStatus0 = true;
        private bool _airMixStatus1 = false;
        private bool _airMixStatus2 = false;
        private bool _airMixStatus3 = false;
        private bool _airMixStatus4 = false;
        private bool _airMixStatus5 = false;
        private bool _airMixStatus6 = false;
        #endregion

        #region Propiedades

        public string DeviceData
        {
            get => _deviceData;
            set => RaisePropertyChanged(ref _deviceData, value);
        }

        public string AmbientTemperature
        {
            get => _ambientTemperature;
            set => RaisePropertyChanged(ref _ambientTemperature, value);
        }
        public string InsideTemperature
        {
            get => _insideTemperature;
            set => RaisePropertyChanged(ref _insideTemperature, value);
        }
        public string InsideHumidity
        {
            get => _insideHumidity;
            set => RaisePropertyChanged(ref _insideHumidity, value);
        }
        public string EvaporatorTemperature
        {
            get => _evaporatorTemperature;
            set => RaisePropertyChanged(ref _evaporatorTemperature, value);
        }
        public string AmbientLight
        {
            get => _ambientLight;
            set => RaisePropertyChanged(ref _ambientLight, value);
        }
        public string DewPointValue
        {
            get => _dewPointValue;
            set => RaisePropertyChanged(ref _dewPointValue, value);
        }
        public string ThermalSensation
        {
            get => _thermalSensation;
            set => RaisePropertyChanged(ref _thermalSensation, value);
        }

        public bool EnabledDefrosterFront
        {
            get => _enableDefrosterFront;
            set => RaisePropertyChanged(ref _enableDefrosterFront, value);
        }
        public bool EnabledDefrostRear
        {
            get => _enableDefrostRear;
            set => RaisePropertyChanged(ref _enableDefrostRear, value);
        }
        public bool EnabledAc
        {
            get => _enableAc;
            set => RaisePropertyChanged(ref _enableAc, value);
        }
        public bool EnabledAuto
        {
            get => _enableAuto;
            set => RaisePropertyChanged(ref _enableAuto, value);
        }
        public bool EnabledAirMode
        {
            get => _enableAirMode;
            set => RaisePropertyChanged(ref _enableAirMode, value);
        }
        public bool EnabledFan1
        {
            get => _enabledFan1;
            set => RaisePropertyChanged(ref _enabledFan1, value);
        }
        public bool EnabledAirFront
        {
            get => _enableAirFront;
            set => RaisePropertyChanged(ref _enableAirFront, value);
        }
        public bool EnabledAirDown
        {
            get => _enableAirDown;
            set => RaisePropertyChanged(ref _enableAirDown, value);
        }
        public bool EnabledVentFront
        {
            get => _enableVentFront;
            set => RaisePropertyChanged(ref _enableVentFront, value);
        }
        public bool EnableBlowerUp
        {
            get => _enableBlowerUp;
            set => RaisePropertyChanged(ref _enableBlowerUp, value);
        }
        public bool EnableBlowerDown
        {
            get => _enableBlowerDown;
            set => RaisePropertyChanged(ref _enableBlowerDown, value);
        }
        public bool EnableAirMixUp
        {
            get => _enableAirMixUp;
            set => RaisePropertyChanged(ref _enableAirMixUp, value);
        }
        public bool EnableAirMixDown
        {
            get => _enableAirMixDown;
            set => RaisePropertyChanged(ref _enableAirMixDown, value);
        }

        public bool Illumination
        {
            get => _illumination;
            set => RaisePropertyChanged(ref _illumination, value);
        }

        public bool ChekedDefrosterFront
        {
            get => _chekedDefrosterFront;
            set => RaisePropertyChanged(ref _chekedDefrosterFront, value);
        }
        public bool ChekedDefrostRear
        {
            get => _chekedDefrostRear;
            set => RaisePropertyChanged(ref _chekedDefrostRear, value);
        }
        public bool ChekedAc
        {
            get => _chekedAc;
            set => RaisePropertyChanged(ref _chekedAc, value);
        }
        public bool ChekedAuto
        {
            get => _chekedAuto;
            set => RaisePropertyChanged(ref _chekedAuto, value);
        }
        public bool ChekedAirMode
        {
            get => _chekedAirMode;
            set => RaisePropertyChanged(ref _chekedAirMode, value);
        }
        public bool ChekedFan1
        {
            get => _chekedFan1;
            set => RaisePropertyChanged(ref _chekedFan1, value);
        }
        public bool ChekedAirFront
        {
            get => _chekedAirFront;
            set => RaisePropertyChanged(ref _chekedAirFront, value);
        }
        public bool ChekedAirDown
        {
            get => _chekedAirDown;
            set => RaisePropertyChanged(ref _chekedAirDown, value);
        }
        public bool ChekedVentFront
        {
            get => _chekedVentFront;
            set => RaisePropertyChanged(ref _chekedVentFront, value);
        }

        public bool ACBlower0
        {
            get => _blowerStatus0;
            set => RaisePropertyChanged(ref _blowerStatus0, value);
        }
        public bool ACBlower1
        {
            get => _blowerStatus1;
            set => RaisePropertyChanged(ref _blowerStatus1, value);
        }
        public bool ACBlower2
        {
            get => _blowerStatus2;
            set => RaisePropertyChanged(ref _blowerStatus2, value);
        }
        public bool ACBlower3
        {
            get => _blowerStatus3;
            set => RaisePropertyChanged(ref _blowerStatus3, value);
        }
        public bool ACBlower4
        {
            get => _blowerStatus4;
            set => RaisePropertyChanged(ref _blowerStatus4, value);
        }
        public bool ACBlower5
        {
            get => _blowerStatus5;
            set => RaisePropertyChanged(ref _blowerStatus5, value);
        }
        public bool ACBlower6
        {
            get => _blowerStatus6;
            set => RaisePropertyChanged(ref _blowerStatus6, value);
        }

        public bool ACAirMix0
        {
            get => _airMixStatus0;
            set => RaisePropertyChanged(ref _airMixStatus0, value);
        }
        public bool ACAirMix1
        {
            get => _airMixStatus1;
            set => RaisePropertyChanged(ref _airMixStatus1, value);
        }
        public bool ACAirMix2
        {
            get => _airMixStatus2;
            set => RaisePropertyChanged(ref _airMixStatus2, value);
        }
        public bool ACAirMix3
        {
            get => _airMixStatus3;
            set => RaisePropertyChanged(ref _airMixStatus3, value);
        }
        public bool ACAirMix4
        {
            get => _airMixStatus4;
            set => RaisePropertyChanged(ref _airMixStatus4, value);
        }
        public bool ACAirMix5
        {
            get => _airMixStatus5;
            set => RaisePropertyChanged(ref _airMixStatus5, value);
        }
        public bool ACAirMix6
        {
            get => _airMixStatus6;
            set => RaisePropertyChanged(ref _airMixStatus6, value);
        }
        #endregion

        #region Comandos
        private DelegateCommand<string> _RelayC_BlowerStatus;
        private DelegateCommand<string> _RelayC_AirMixStatus;
        private DelegateCommand _RelayC_Fan1;
        private DelegateCommand _RelayC_BlowerUp;
        private DelegateCommand _RelayC_BlowerDown;
        private DelegateCommand _RelayC_AirMixUp;
        private DelegateCommand _RelayC_AirMixDown;
        private DelegateCommand _RelayC_DefrosterFront;
        private DelegateCommand _RelayC_DefrostRear;
        private DelegateCommand _RelayC_Ac;
        private DelegateCommand _RelayC_Auto;
        private DelegateCommand _RelayC_AirMode;
        private DelegateCommand _RelayC_AirFront;
        private DelegateCommand _RelayC_AirDown;
        private DelegateCommand _RelayC_VentFront;

        public ICommand RelayC_BlowerStatus => _RelayC_BlowerStatus = _RelayC_BlowerStatus ?? new DelegateCommand<string>(StatusBlower);
        public ICommand RelayC_AirMixStatus => _RelayC_AirMixStatus = _RelayC_AirMixStatus ?? new DelegateCommand<string>(StatusAirMix);
        public ICommand RelayC_Fan1 => _RelayC_Fan1 = _RelayC_Fan1 ?? new DelegateCommand(Fan1);
        public ICommand RelayC_BlowerUp => _RelayC_BlowerUp = _RelayC_BlowerUp ?? new DelegateCommand(BlowerUp);
        public ICommand RelayC_BlowerDown => _RelayC_BlowerDown = _RelayC_BlowerDown ?? new DelegateCommand(BlowerDown);
        public ICommand RelayC_AirMixUp => _RelayC_AirMixUp = _RelayC_AirMixUp ?? new DelegateCommand(AirMixUp);
        public ICommand RelayC_AirMixDown => _RelayC_AirMixDown = _RelayC_AirMixDown ?? new DelegateCommand(AirMixDown);
        public ICommand RelayC_DefrosterFront => _RelayC_DefrosterFront = _RelayC_DefrosterFront ?? new DelegateCommand(DefrosterFront);
        public ICommand RelayC_DefrostRear => _RelayC_DefrostRear = _RelayC_DefrostRear ?? new DelegateCommand(DefrostRear);
        public ICommand RelayC_Ac => _RelayC_Ac = _RelayC_Ac ?? new DelegateCommand(Ac);
        public ICommand RelayC_Auto => _RelayC_Auto = _RelayC_Auto ?? new DelegateCommand(Auto);
        public ICommand RelayC_AirMode => _RelayC_AirMode = _RelayC_AirMode ?? new DelegateCommand(AirMode);
        public ICommand RelayC_AirFront => _RelayC_AirFront = _RelayC_AirFront ?? new DelegateCommand(AirFront);
        public ICommand RelayC_AirDown => _RelayC_AirDown = _RelayC_AirDown ?? new DelegateCommand(AirDown);
        public ICommand RelayC_VentFront => _RelayC_VentFront = _RelayC_VentFront ?? new DelegateCommand(VentFront);
        #endregion

        #region Metodos/Funciones
        public ClimateControlViewModel(ISerialDeviceService serialDeviceService, ILocalAppDataService localAppDataService, IDialogService dialogService, ILoaderService loaderService)
        {
            _serialDeviceService = serialDeviceService;
            _localAppDataService = localAppDataService;
            _dialogService = dialogService;
            _loaderService = loaderService;

            device = new DeviceModel();

            _serialDeviceService.NewDataDevice += _serialDeviceService_NewDataDevice;
        }

        private void _serialDeviceService_NewDataDevice(object sender, EventArgs e)
        {
            string str_dataInt = (string)sender;
            DeviceData = str_dataInt;
            str_data = str_dataInt.Split(str_Delimiter, StringSplitOptions.RemoveEmptyEntries);
            if (str_data[0] == "<")
            {
                for (byte i = 1; i < str_data.Length; i += 1)
                {
                    switch (str_data[i])
                    {
                        case "I":
                            if (str_data[i + 1] == "1")
                            {
                                Illumination = true;
                                break;
                            }
                            else
                            {
                                Illumination = false;
                                break;
                            }
                        case "Te":
                            EvaporatorTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Ta":
                            AmbientTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Ti":
                            InsideTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Ts":
                            ThermalSensation = str_data[i + 1];
                            break;
                        case "H":
                            InsideHumidity = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Pr":
                            DewPointValue = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "La":
                            AmbientLight = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "F1":
                            if (str_data[i + 1] == "1")
                            {
                                ChekedFan1 = true;
                                break;
                            }
                            else
                            {
                                ChekedFan1 = false;
                                break;
                            }
                        case "Bl":
                            StatusBlower(str_data[i + 1]);
                            break;
                        case "Co":
                            if (str_data[i + 1] == "1")
                            {
                                ChekedAc = true;
                                break;
                            }
                            else
                            {
                                ChekedAc = false;
                                break;
                            }

                        default:
                            break;
                    }
                }
            }
        }

        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            LoadLocalData();
            _serialDeviceService.DeviceCheck(device);
            return null;
        }

        /// <summary>
        /// Optiene los datos almacenados localmente del dispositivo
        /// </summary>
        private void LoadLocalData()
        {
            bool hasContainer = _localAppDataService.ContainerLocalAppDataExists(LocalDataType.DeviceData);
            if (hasContainer)
            {
                ApplicationDataCompositeValue compositeValues = _localAppDataService.GetContainerLocalAppData(LocalDataType.DeviceData);

                device.Name = (string)compositeValues["DeviceName"];
                device.Parity = (string)compositeValues["Parity"];
                device.StopBit = (string)compositeValues["StopBit"];
                device.HandShake = (string)compositeValues["HandShake"];
                device.DataBits = (ushort)compositeValues["DataBits"];
                device.BaudRate = (uint)compositeValues["BaudRate"];
            }
            else
            {
                _dialogService.StatusNotification(_loaderService.GetString("DeviceNotConfigured"), NotifyType.Information);
            }
        }

        private void BlowerUp()
        {
            if (value_Blower < 6)
            {
                value_Blower += 1;
                StatusBlower(Convert.ToString(value_Blower));
            }
        }
        private void BlowerDown()
        {
            if (value_Blower > 0)
            {
                value_Blower -= 1;
                StatusBlower(Convert.ToString(value_Blower));
            }
        }
        private void StatusBlower(string value)
        {
            switch (value)
            {
                case "0":
                    value_Blower = 0;
                    ACBlower0 = true;
                    break;

                case "1":
                    value_Blower = 1;
                    ACBlower1 = true;
                    break;

                case "2":
                    value_Blower = 2;
                    ACBlower2 = true;
                    break;

                case "3":
                    value_Blower = 3;
                    ACBlower3 = true;
                    break;

                case "4":
                    value_Blower = 4;
                    ACBlower4 = true;
                    break;

                case "5":
                    value_Blower = 5;
                    ACBlower5 = true;
                    break;

                case "6":
                    value_Blower = 6;
                    ACBlower6 = true;
                    break;

                default:
                    break;
            }
            if (value_Blower > 0)
            {
                EnabledAc = true;
            }
            else
            {
                EnabledAc = false;
                ChekedAc = false;
            }
            SentDataPort("b" + "," + value_Blower);
        }
        private void AirMixUp()
        {
            if (value_AirMix < 6)
            {
                value_AirMix += 1;
                StatusAirMix(Convert.ToString(value_AirMix));
            }
        }
        private void AirMixDown()
        {
            if (value_AirMix > 0)
            {
                value_AirMix -= 1;
                StatusAirMix(Convert.ToString(value_AirMix));
            }
        }
        private void StatusAirMix(string value)
        {
            switch (value)
            {
                case "0":
                    value_AirMix = 0;
                    ACAirMix0 = true;
                    break;

                case "1":
                    value_AirMix = 1;
                    ACAirMix1 = true;
                    break;

                case "2":
                    value_AirMix = 2;
                    ACAirMix2 = true;
                    break;

                case "3":
                    value_AirMix = 3;
                    ACAirMix3 = true;
                    break;

                case "4":
                    value_AirMix = 4;
                    ACAirMix4 = true;
                    break;

                case "5":
                    value_AirMix = 5;
                    ACAirMix5 = true;
                    break;

                case "6":
                    value_AirMix = 6;
                    ACAirMix6 = true;
                    break;

                default:
                    break;
            }
            SentDataPort("m" + "," + value_AirMix);
        }
        private void DefrosterFront()
        {
            if (ChekedDefrosterFront)
            {
                SentDataPort("e,1");
            }
            else
            {
                SentDataPort("e,0");
            }
        }
        private void DefrostRear()
        {
            if (ChekedDefrostRear)
            {
                SentDataPort("d,1");
            }
            else
            {
                SentDataPort("d,0");
            }
        }
        private void Ac()
        {
            if (ChekedAc)
            {
                SentDataPort("a,1");
            }
            else
            {
                SentDataPort("a,0");
            }
        }
        private void Auto()
        {
            if (ChekedAuto)
            {
                SentDataPort("z,1");
            }
            else
            {
                SentDataPort("z,0");
            }
        }
        private void AirMode()
        {
            if (ChekedAirMode)
            {
                SentDataPort("f,0");
            }
            else
            {
                SentDataPort("f,1");
            }
        }
        private void AirFront()
        {
            if (ChekedAirFront)
            {
                SentDataPort("v,5");
                ChekedAirDown = false;
                ChekedVentFront = false;
            }
        }
        private void AirDown()
        {
            if (ChekedAirDown)
            {
                SentDataPort("v,3");
                ChekedVentFront = false;
                ChekedAirFront = false;
            }
        }
        private void VentFront()
        {
            if (ChekedVentFront)
            {
                SentDataPort("v,1");
                ChekedAirFront = false;
                ChekedAirDown = false;
            }
        }

        private void SentDataPort(string value)
        {
            string string_data;
            string_data = char_StartMarker + value + char_EndMarker;
            _serialDeviceService.WriteToPort(string_data);
        }

        private void Fan1()
        {

        }
        #endregion
    }
}
