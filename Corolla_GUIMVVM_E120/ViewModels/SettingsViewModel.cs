using Corolla_GUIMVVM_E120.Services.DialogService;
using Corolla_GUIMVVM_E120.Services.LocalAppDataService;
using Corolla_GUIMVVM_E120.Services.LoaderService;
using Corolla_GUIMVVM_E120.ViewModels.Base;
using Corolla_GUIMVVM_E120.Models;
using Corolla_GUIMVVM_E120.Contants;

using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;

namespace Corolla_GUIMVVM_E120.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Atributos

        private ILoaderService _loaderService;
        private ILocalAppDataService _localAppDataService;

        private SuspendingEventHandler appSuspendEventHandler;
        private EventHandler<object> appResumeEventHandler;

        private readonly DeviceModel localDataDevice;
        private readonly DeviceModel selectDevice;

        private readonly Dictionary<DeviceWatcher, string> mapDeviceWatchersToDeviceSelector;
        private bool watchersSuspended;
        private bool watchersStarted;

        
        public ObservableCollection<DeviceListEntry> ListOfDevices
        {
            get => _listOfDevices;
            set => SetProperty(ref _listOfDevices, value);
        }

        #endregion

        #region Propiedades

        private uint _baudRateDeviceSelect;

        private ushort _dataBitsDeviceSelect;

        private bool _autoReconectSelect;
        private bool _breakSignalStateSelect;
        private bool _dataTerminalReadyEnabledSelect;
        private bool _requestToSendEnabledSelect;

        private string _nameDeviceSelect;
        private string _parityDeviceSelect;
        private string _stopBitDeviceSelect;
        private string _handShakeDeviceSelect;

        private ObservableCollection<DeviceListEntry> _listOfDevices;
        private List<uint> _listOfBaudRate;
        private List<string> _listOfParity;
        private List<string> _listOfStopBit;
        private List<string> _listOfHandShake;
        private List<ushort> _listOfDataBits;

        public uint BaudRateDeviceSelect
        {
            get => _baudRateDeviceSelect;
            set => SetProperty(ref _baudRateDeviceSelect, value);
        }

        public ushort DataBitsDeviceSelect
        {
            get => _dataBitsDeviceSelect;
            set => SetProperty(ref _dataBitsDeviceSelect, value);
        }

        public bool AutoReconectSelect
        {
            get => _autoReconectSelect;
            set => SetProperty(ref _autoReconectSelect, value);
        }
        public bool BreakSignalStateSelect
        {
            get => _breakSignalStateSelect;
            set => SetProperty(ref _breakSignalStateSelect, value);
        }
        public bool DataTerminalReadyEnabledSelect
        {
            get => _dataTerminalReadyEnabledSelect;
            set => SetProperty(ref _dataTerminalReadyEnabledSelect, value);
        }
        public bool RequestToSendEnabledSelect
        {
            get => _requestToSendEnabledSelect;
            set => SetProperty(ref _requestToSendEnabledSelect, value);
        }

        public List<uint> ListOfBaudRate
        {
            get => _listOfBaudRate;
            set => SetProperty(ref _listOfBaudRate, value);
        }
        public List<string> ListOfParity
        {
            get => _listOfParity;
            set => SetProperty(ref _listOfParity, value);
        }
        public List<string> ListOfStopBit
        {
            get => _listOfStopBit;
            set => SetProperty(ref _listOfStopBit, value);
        }
        public List<string> ListOfHandShake
        {
            get => _listOfHandShake;
            set => SetProperty(ref _listOfHandShake, value);
        }
        public List<ushort> ListOfDataBits
        {
            get => _listOfDataBits;
            set => SetProperty(ref _listOfDataBits, value);
        }
        public string NameDeviceSelect
        {
            get => _nameDeviceSelect;
            set => SetProperty(ref _nameDeviceSelect, value);
        }
        public string ParityDeviceSelect
        {
            get => _parityDeviceSelect;
            set => SetProperty(ref _parityDeviceSelect, value);
        }
        public string StopBitDeviceSelect
        {
            get => _stopBitDeviceSelect;
            set => SetProperty(ref _stopBitDeviceSelect, value);
        }
        public string HandShakeDeviceSelect
        {
            get => _handShakeDeviceSelect;
            set => SetProperty(ref _handShakeDeviceSelect, value);
        }

        #endregion

        public SettingsViewModel(ILoaderService loaderService, ILocalAppDataService localAppDataService)
        {
            _loaderService = loaderService;
            _localAppDataService = localAppDataService;

            localDataDevice = new DeviceModel();
            selectDevice = new DeviceModel();
            mapDeviceWatchersToDeviceSelector = new Dictionary<DeviceWatcher, string>();

            ListOfDevices = new ObservableCollection<DeviceListEntry>();
            ListOfBaudRate = new List<uint>
            {
                9600,
                14400,
                19200,
                38400,
                56000,
                57600,
                115200,
                250000
            };
            ListOfParity = new List<string>
            {
                "None",
                "Even",
                "Odd",
                "Mark",
                "Space"
            };
            ListOfStopBit = new List<string>
            {
                "One",
                "OnePointFive",
                "Two"
            };
            ListOfHandShake = new List<string>
            {
                "None",
                "RequestToSend",
                "XOnXOff",
                "RequestToSendXOnXOff"
            };
            ListOfDataBits = new List<ushort>
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
                8,
            };

            watchersStarted = false;
            watchersSuspended = false;
        }

        /// <summary>
        /// Desregístrate de los eventos de la App y de los eventos de DeviceWatcher 
        /// porque esta página se descargará.
        /// </summary>
        /// <param name="eventArgs"></param>
        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            StopDeviceWatchers();
            StopHandlingAppEvents();

            SaveDeviceData();
            ClearAllData();
            return null;
        }

        /// <summary>
        /// Se invoca cuando esta página está a punto de mostrarse en un Frame.
        /// Crea los objetos DeviceWatcher cuando el usuario navega a esta página 
        /// para que se rellene la lista de dispositivos de la UI.
        /// </summary>
        /// <param name="e"></param>Datos del evento que describen cómo se llegó a esta página.  
        /// El parámetro propiedad se utiliza normalmente para configurar la página.</param>
        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            // Empieza a monitorizar los acontecimientos
            StartHandlingAppEvents();

            // Inicializar los observadores de dispositivos deseados para que podamos vigilar cuando los
            // dispositivos se conectan/retiran
            InitializeDeviceWatchers();
            StartDeviceWatchers();

            LoadDeviceData();
            return null;
        }

        #region Metodos

        private void LoadDeviceData()
        {
            if (_localAppDataService.ContainerLocalAppDataExists(LocalDataType.DeviceData))
            {
                ApplicationDataCompositeValue compositeValues = _localAppDataService.GetContainerLocalAppData(LocalDataType.DeviceData);

                localDataDevice.Name = (string)compositeValues["DeviceName"];
                localDataDevice.Parity = (string)compositeValues["Parity"];
                localDataDevice.StopBit = (string)compositeValues["StopBit"];
                localDataDevice.HandShake = (string)compositeValues["HandShake"];
                localDataDevice.DataBits = (ushort)compositeValues["DataBits"];
                localDataDevice.BaudRate = (uint)compositeValues["BaudRate"];
                localDataDevice.AutoReconect = (bool)compositeValues["AutoReconect"];
                localDataDevice.BreakSignalState = (bool)compositeValues["BreakSignalState"];
                localDataDevice.DataTerminalReadyEnabled = (bool)compositeValues["DataTerminalReadyEnabled"];
                localDataDevice.RequestToSendEnabled = (bool)compositeValues["RequestToSendEnabled"];
            }
            else
            {
                _localAppDataService.CreateContainerLocalAppData(LocalDataType.DeviceData);
            }

            NameDeviceSelect = localDataDevice.Name;
            ParityDeviceSelect = localDataDevice.Parity;
            StopBitDeviceSelect = localDataDevice.StopBit;
            HandShakeDeviceSelect = localDataDevice.HandShake;
            DataBitsDeviceSelect = localDataDevice.DataBits;
            BaudRateDeviceSelect = localDataDevice.BaudRate;
            AutoReconectSelect = localDataDevice.AutoReconect;
            BreakSignalStateSelect = localDataDevice.BreakSignalState;
            DataTerminalReadyEnabledSelect = localDataDevice.DataTerminalReadyEnabled;
            RequestToSendEnabledSelect = localDataDevice.RequestToSendEnabled;
        }

        private void SaveDeviceData()
        {
            selectDevice.Name = NameDeviceSelect;
            selectDevice.Parity = ParityDeviceSelect;
            selectDevice.StopBit = StopBitDeviceSelect;
            selectDevice.HandShake = HandShakeDeviceSelect;
            selectDevice.DataBits = DataBitsDeviceSelect;
            selectDevice.BaudRate = BaudRateDeviceSelect;
            selectDevice.AutoReconect = AutoReconectSelect;
            selectDevice.BreakSignalState = BreakSignalStateSelect;
            selectDevice.DataTerminalReadyEnabled = DataTerminalReadyEnabledSelect;
            selectDevice.RequestToSendEnabled = RequestToSendEnabledSelect;

            if (localDataDevice.Equals(selectDevice))
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue
                {
                    ["DeviceName"] = selectDevice.Name,
                    ["Parity"] = selectDevice.Parity,
                    ["StopBit"] = selectDevice.StopBit,
                    ["HandShake"] = selectDevice.HandShake,
                    ["DataBits"] = selectDevice.DataBits,
                    ["BaudRate"] = selectDevice.BaudRate,
                    ["AutoReconect"] = selectDevice.AutoReconect,
                    ["BreakSignalState"] = selectDevice.BreakSignalState,
                    ["DataTerminalReadyEnabled"] = selectDevice.DataTerminalReadyEnabled,
                    ["RequestToSendEnabled"] = selectDevice.RequestToSendEnabled
                };
                _localAppDataService.SetCompositeLocalAppData(LocalDataType.DeviceData, composite);
            }
        }

        private void ClearAllData()
        {
            ListOfBaudRate.Clear();
            ListOfDataBits.Clear();
            ListOfHandShake.Clear();
            ListOfParity.Clear();
            ListOfStopBit.Clear();
            ListOfDevices.Clear();
        }

        /// <summary>
        /// Inicializa los observadores de dispositivos para vigilar los dispositivos seriales.
        /// GetDeviceSelector devuelve una cadena AQS que se puede pasar directamente 
        /// a DeviceWatcher.createWatcher() o DeviceInformation.createFromIdAsync(). 
        /// <summary>
        private void InitializeDeviceWatchers()
        {
            // Apunta a todos los Dispositivos Seriales presentes en el sistema
            string deviceSelector = SerialDevice.GetDeviceSelector();
            // Crear un observador de dispositivos para buscar instancias del Dispositivo Serial que coincidan
            // con el selector de dispositivos utilizado anteriormente.
            DeviceWatcher deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);
            // Permitir que el EventHandlerForDevice maneje los eventos del observador del dispositivo que se relacionan o afectan a nuestro dispositivo
            // (es decir, la eliminación del dispositivo, la adición, la suspensión/reanudación de la aplicación)
            AddDeviceWatcher(deviceWatcher, deviceSelector);
        }

        private void StartHandlingAppEvents()
        {
            appSuspendEventHandler = new SuspendingEventHandler(OnAppSuspension);
            appResumeEventHandler = new EventHandler<object>(OnAppResume);

            // Estos evento se lanza cuando se sale de la aplicación y cuando se suspende la aplicación
            Application.Current.Suspending += appSuspendEventHandler;
            Application.Current.Resuming += appResumeEventHandler;
        }

        private void StopHandlingAppEvents()
        {
            // Este evento se lanza cuando se sale de la aplicación y cuando se suspende la aplicación
            Application.Current.Suspending -= appSuspendEventHandler;
            Application.Current.Resuming -= appResumeEventHandler;
        }

        /// <summary>
        /// Debemos detener los DeviceWatchers porque los observadores de dispositivos seguirán lanzando 
        /// eventos aunque la app está en suspensión, lo cual no es deseado (drena la batería).
        /// Reanudamos el device watcher una vez que la app se reanude de nuevo.
        /// <summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void OnAppSuspension(object sender, SuspendingEventArgs args)
        {
            if (watchersStarted)
            {
                watchersSuspended = true;
                StopDeviceWatchers();
            }
            else
            {
                watchersSuspended = false;
            }
        }

        /// <summary>
        /// Ver OnAppSuspension para saber por qué iniciamos de nuevo los vigilantes de los dispositivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnAppResume(object sender, object args)
        {
            if (watchersSuspended)
            {
                watchersSuspended = false;
                StartDeviceWatchers();
            }
        }

        /// <sumário>
        /// Inicia todos los observadores de dispositivos, incluindo los que fueron
        /// individualmente parados.
        /// </sumário>
        /// 
        private void StartDeviceWatchers()
        {
            // Iniciar todos os observadores de dispositivos
            watchersStarted = true;
            foreach (DeviceWatcher deviceWatcher in mapDeviceWatchersToDeviceSelector.Keys)
            {
                if ((deviceWatcher.Status != DeviceWatcherStatus.Started)
                    && (deviceWatcher.Status != DeviceWatcherStatus.EnumerationCompleted))
                {
                    deviceWatcher.Start();
                }
            }
        }

        /// <summary>
        /// Detiene todos los observadores de dispositivos.
        /// </summary>
        private void StopDeviceWatchers()
        {
            // Parar todos los observadores del dispositivos
            foreach (DeviceWatcher deviceWatcher in mapDeviceWatchersToDeviceSelector.Keys)
            {
                if ((deviceWatcher.Status == DeviceWatcherStatus.Started)
                    || (deviceWatcher.Status == DeviceWatcherStatus.EnumerationCompleted))
                {
                    deviceWatcher.Stop();
                }
            }
            watchersStarted = false;
        }

        private void AddDeviceWatcher(DeviceWatcher deviceWatcher, string deviceSelector)
        {
            deviceWatcher.Added += new TypedEventHandler<DeviceWatcher, DeviceInformation>(OnDeviceAdded);
            deviceWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(OnDeviceRemoved);
            mapDeviceWatchersToDeviceSelector.Add(deviceWatcher, deviceSelector);
        }

        /// <summary>
        /// Esta función añadirá el dispositivo a la listOfDevices para que aparezca en la UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="deviceInformation"></param>
        private async void OnDeviceAdded(DeviceWatcher sender, DeviceInformation deviceInformation)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    AddDeviceToList(deviceInformation, mapDeviceWatchersToDeviceSelector[sender]);
                });
        }

        /// <summary>
        /// Eliminaremos el dispositivo de la interfaz de usuario.
        /// <summary>
        /// <param name="sender"></param>
        /// <param name="deviceInformationUpdate"></param>
        private async void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInformationUpdate)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                RemoveDeviceFromList(deviceInformationUpdate.Id);
            });
        }

        /// <summary>
        /// Crea una DeviceListEntry para un dispositivo y la añade a la lista de dispositivos en la UI.
        /// <summary>
        /// <param name="deviceInformation">Información del dispositivo que se va a añadir a la lista</param>
        /// <param name="deviceSelector">El AQS utilizado para encontrar este dispositivo</param>
        private void AddDeviceToList(DeviceInformation deviceInformation, string deviceSelector)
        {
            // buscar en la lista de dispositivos un dispositivo con un ID de interfaz que coincida
            DeviceListEntry match = FindDevice(deviceInformation.Id);

            // Añadir el dispositivo si es nuevo
            if (match == null)
            {
                // Crear un nuevo elemento para esta interfaz de dispositivo, y poner en cola la consulta de su
                // información del dispositivo
                match = new DeviceListEntry(deviceInformation, deviceSelector);

                // Añadir el nuevo elemento al final de la lista de dispositivos
                ListOfDevices.Add(match);
            }
        }

        private void RemoveDeviceFromList(string deviceId)
        {
            // Elimina la entrada del dispositivo de la lista interal; por lo tanto la UI
            DeviceListEntry deviceEntry = FindDevice(deviceId);
            _ = ListOfDevices.Remove(deviceEntry);
        }

        /// <summary>
        /// Busca en la lista de dispositivos existente la primera DeviceListEntry que tenga
        /// el Id de dispositivo especificado.
        /// </summary>
        /// <param name="deviceId">Id del dispositivo que se está buscando</param>
        /// <returns>Entrada de la lista de dispositivos que tiene el Id proporcionado; si no, un nullptr</returns>
        private DeviceListEntry FindDevice(string deviceId)
        {
            if (deviceId != null)
            {
                foreach (DeviceListEntry entry in ListOfDevices)
                {
                    if (entry.DeviceInformation.Id == deviceId)
                    {
                        return entry;
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
