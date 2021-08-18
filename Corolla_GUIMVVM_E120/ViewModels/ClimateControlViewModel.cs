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

        public ClimateControlModel control;

        private const char char_StartMarker = '<';
        private const char char_EndMarker = '>';
        private readonly string[] str_Delimiter = new string[] { " ", ",", ":" };
        public string[] str_data;

        private byte value_Blower = 0;
        private byte value_AirMix = 0;

        private bool bool_BlowerStatus0 = true;
        private bool bool_BlowerStatus1 = false;
        private bool bool_BlowerStatus2 = false;
        private bool bool_BlowerStatus3 = false;
        private bool bool_BlowerStatus4 = false;
        private bool bool_BlowerStatus5 = false;
        private bool bool_BlowerStatus6 = false;

        private bool bool_AirMix0 = true;
        private bool bool_AirMix1 = false;
        private bool bool_AirMix2 = false;
        private bool bool_AirMix3 = false;
        private bool bool_AirMix4 = false;
        private bool bool_AirMix5 = false;
        private bool bool_AirMix6 = false;
        #endregion

        #region Propiedades
        private string _deviceData = null;

        public string DeviceData
        {
            get => _deviceData;
            set => RaisePropertyChanged(ref _deviceData, value);
        }

        public bool ACBlower0
        {
            get => bool_BlowerStatus0;
            set => RaisePropertyChanged(ref bool_BlowerStatus0, value);
        }
        public bool ACBlower1
        {
            get => bool_BlowerStatus1;
            set => RaisePropertyChanged(ref bool_BlowerStatus1, value);
        }
        public bool ACBlower2
        {
            get => bool_BlowerStatus2;
            set => RaisePropertyChanged(ref bool_BlowerStatus2, value);
        }
        public bool ACBlower3
        {
            get => bool_BlowerStatus3;
            set => RaisePropertyChanged(ref bool_BlowerStatus3, value);
        }
        public bool ACBlower4
        {
            get => bool_BlowerStatus4;
            set => RaisePropertyChanged(ref bool_BlowerStatus4, value);
        }
        public bool ACBlower5
        {
            get => bool_BlowerStatus5;
            set => RaisePropertyChanged(ref bool_BlowerStatus5, value);
        }
        public bool ACBlower6
        {
            get => bool_BlowerStatus6;
            set => RaisePropertyChanged(ref bool_BlowerStatus6, value);
        }

        public bool ACAirMix0
        {
            get => bool_AirMix0;
            set => RaisePropertyChanged(ref bool_AirMix0, value);
        }
        public bool ACAirMix1
        {
            get => bool_AirMix1;
            set => RaisePropertyChanged(ref bool_AirMix1, value);
        }
        public bool ACAirMix2
        {
            get => bool_AirMix2;
            set => RaisePropertyChanged(ref bool_AirMix2, value);
        }
        public bool ACAirMix3
        {
            get => bool_AirMix3;
            set => RaisePropertyChanged(ref bool_AirMix3, value);
        }
        public bool ACAirMix4
        {
            get => bool_AirMix4;
            set => RaisePropertyChanged(ref bool_AirMix4, value);
        }
        public bool ACAirMix5
        {
            get => bool_AirMix5;
            set => RaisePropertyChanged(ref bool_AirMix5, value);
        }
        public bool ACAirMix6
        {
            get => bool_AirMix6;
            set => RaisePropertyChanged(ref bool_AirMix6, value);
        }

        private void Fan1()
        {

        }
        
        

        private void BlowerUpAsync()
        {
            if (value_Blower < 6)
            {
                value_Blower += 1;
                StatusBlowerAsync(Convert.ToString(value_Blower));
            }
        }
        private void BlowerDownAsync()
        {
            if (value_Blower > 0)
            {
                value_Blower -= 1;
                StatusBlowerAsync(Convert.ToString(value_Blower));
            }
        }
        private void StatusBlowerAsync(string value)
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
                control.EnabledAc = true;
            }
            else
            {
                control.EnabledAc = false;
                control.ChekedAc = false;
            }
            SentDataPort("b" + "," + value_Blower);
        }

        private void AirMixUpAsync()
        {
            if (value_AirMix < 6)
            {
                value_AirMix += 1;
                StatusAirMixAsync(Convert.ToString(value_AirMix));
            }
        }
        private void AirMixDownAsync()
        {
            if (value_AirMix > 0)
            {
                value_AirMix -= 1;
                StatusAirMixAsync(Convert.ToString(value_AirMix));
            }
        }
        private void StatusAirMixAsync(string value)
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
            if (control.ChekedDefrosterFront)
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
            if (control.ChekedDefrostRear)
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
            if (control.ChekedAc)
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
            if (control.ChekedAuto)
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
            if (control.ChekedAirMode)
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
            if (control.ChekedAirFront)
            {
                SentDataPort("v,5");
                control.ChekedAirDown = false;
                control.ChekedVentFront = false;
            }
        }
        private void AirDown()
        {
            if (control.ChekedAirDown)
            {
                SentDataPort("v,3");
                control.ChekedVentFront = false;
                control.ChekedAirFront = false;
            }
        }
        private void VentFront()
        {
            if (control.ChekedVentFront)
            {
                SentDataPort("v,1");
                control.ChekedAirFront = false;
                control.ChekedAirDown = false;
            }
        }
        #endregion

        #region Metodos/Funciones
        public ClimateControlViewModel(ISerialDeviceService serialDeviceService, ILocalAppDataService localAppDataService, IDialogService dialogService, ILoaderService loaderService)
        {
            _serialDeviceService = serialDeviceService;
            _localAppDataService = localAppDataService;
            _dialogService = dialogService;
            _loaderService = loaderService;

            device = new DeviceModel();
            control = new ClimateControlModel();

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
                                control.Illumination = true;
                                break;
                            }
                            else
                            {
                                control.Illumination = false;
                                break;
                            }
                        case "Te":
                            control.EvaporatorTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Ta":
                            control.AmbientTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Ti":
                            control.InsideTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Ts":
                            control.ThermalSensation = str_data[i + 1];
                            break;
                        case "H":
                            control.InsideHumidity = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "Pr":
                            control.DewPointValue = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "La":
                            control.AmbientLight = Convert.ToString(short.Parse(str_data[i + 1]));
                            break;
                        case "F1":
                            if (str_data[i + 1] == "1")
                            {
                                control.ChekedFan1 = true;
                                break;
                            }
                            else
                            {
                                control.ChekedFan1 = false;
                                break;
                            }
                        case "Bl":
                            StatusBlowerAsync(str_data[i + 1]);
                            break;
                        case "Co":
                            if (str_data[i + 1] == "1")
                            {
                                control.ChekedAc = true;
                                break;
                            }
                            else
                            {
                                control.ChekedAc = false;
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


        private DelegateCommand<string> _RelayC_BlowerStatus;
        public ICommand RelayC_BlowerStatus
        {
            get { return _RelayC_BlowerStatus = _RelayC_BlowerStatus ?? new DelegateCommand<string>(StatusBlowerAsync); }
        }
        private DelegateCommand<string> _RelayC_AirMixStatus;
        public ICommand RelayC_AirMixStatus
        {
            get { return _RelayC_AirMixStatus = _RelayC_AirMixStatus ?? new DelegateCommand<string>(StatusAirMixAsync); }
        }

        private DelegateCommand _warningLigth;
        public ICommand WarningLigth
        {
            get { return _warningLigth = _warningLigth ?? new DelegateCommand(WarningLigthCommandExecute); }
        }

        private DelegateCommand _RelayC_Fan1;
        public ICommand RelayC_Fan1
        {
            get { return _RelayC_Fan1 = _RelayC_Fan1 ?? new DelegateCommand(Fan1); }
        }

        private DelegateCommand _RelayC_BlowerUp;
        public ICommand RelayC_BlowerUp
        {
            get { return _RelayC_BlowerUp = _RelayC_BlowerUp ?? new DelegateCommand(BlowerUpAsync); }
        }

        private DelegateCommand _RelayC_BlowerDown;
        public ICommand RelayC_BlowerDown
        {
            get { return _RelayC_BlowerDown = _RelayC_BlowerDown ?? new DelegateCommand(BlowerDownAsync); }
        }

        private DelegateCommand _RelayC_AirMixUp;
        public ICommand RelayC_AirMixUp
        {
            get { return _RelayC_AirMixUp = _RelayC_AirMixUp ?? new DelegateCommand(AirMixUpAsync); }
        }

        private DelegateCommand _RelayC_AirMixDown;
        public ICommand RelayC_AirMixDown
        {
            get { return _RelayC_AirMixDown = _RelayC_AirMixDown ?? new DelegateCommand(AirMixDownAsync); }
        }

        private DelegateCommand _RelayC_DefrosterFront;
        public ICommand RelayC_DefrosterFront
        {
            get { return _RelayC_DefrosterFront = _RelayC_DefrosterFront ?? new DelegateCommand(DefrosterFront); }
        }

        private DelegateCommand _RelayC_DefrostRear;
        public ICommand RelayC_DefrostRear
        {
            get { return _RelayC_DefrostRear = _RelayC_DefrostRear ?? new DelegateCommand(DefrostRear); }
        }

        private DelegateCommand _RelayC_Ac;
        public ICommand RelayC_Ac
        {
            get { return _RelayC_Ac = _RelayC_Ac ?? new DelegateCommand(Ac); }
        }

        private DelegateCommand _RelayC_Auto;
        public ICommand RelayC_Auto
        {
            get { return _RelayC_Auto = _RelayC_Auto ?? new DelegateCommand(Auto); }
        }

        private DelegateCommand _RelayC_AirMode;
        public ICommand RelayC_AirMode
        {
            get { return _RelayC_AirMode = _RelayC_AirMode ?? new DelegateCommand(AirMode); }
        }

        private DelegateCommand _RelayC_AirFront;
        public ICommand RelayC_AirFront
        {
            get { return _RelayC_AirFront = _RelayC_AirFront ?? new DelegateCommand(AirFront); }
        }

        private DelegateCommand _RelayC_AirDown;
        public ICommand RelayC_AirDown
        {
            get { return _RelayC_AirDown = _RelayC_AirDown ?? new DelegateCommand(AirDown); }
        }

        private DelegateCommand _RelayC_VentFront;
        public ICommand RelayC_VentFront
        {
            get { return _RelayC_VentFront = _RelayC_VentFront ?? new DelegateCommand(VentFront); }
        }

        private void WarningLigthCommandExecute()
        {
            if (control.ChekedWarningLigth)
            {
                SentDataPort("w,1");
            }
            else
            {
                SentDataPort("w,0");
            }
        }
        private void SentDataPort(string value)
        {
            string string_data;
            string_data = char_StartMarker + value + char_EndMarker;
            _serialDeviceService.WriteToPort(string_data);
        }
        #endregion
    }
}
