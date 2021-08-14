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
    public class SettingsViewModel : ViewModelPageBase
    {
        #region Variables

        #region Declaracion de Interfaces
        private ILoaderService _loaderService;
        private ILocalAppDataService _localAppDataService;
        #endregion

        #region Declaracion de Eventos
        private SuspendingEventHandler appSuspendEventHandler;
        private EventHandler<object> appResumeEventHandler;
        #endregion

        #region Declaracion de Clases
        public DeviceModel localDataDevice;
        public DeviceModel selectDevice;
        #endregion

        private Dictionary<DeviceWatcher, string> mapDeviceWatchersToDeviceSelector;
        private bool watchersSuspended;
        private bool watchersStarted;

        private ObservableCollection<DeviceListEntry> _listOfDevices;
        public ObservableCollection<DeviceListEntry> ListOfDevices
        {
            get => _listOfDevices;
            set => RaisePropertyChanged(ref _listOfDevices, value);
        }

        private List<uint> _listOfBaudRate;
        public List<uint> ListOfBaudRate
        {
            get => _listOfBaudRate;
            set => RaisePropertyChanged(ref _listOfBaudRate, value);
        }

        private List<string> _listOfParity;
        public List<string> ListOfParity
        {
            get => _listOfParity;
            set => RaisePropertyChanged(ref _listOfParity, value);
        }

        private List<string> _listOfStopBit;
        public List<string> ListOfStopBit
        {
            get => _listOfStopBit;
            set => RaisePropertyChanged(ref _listOfStopBit, value);
        }

        private List<string> _listOfHandShake;
        public List<string> ListOfHandShake
        {
            get => _listOfHandShake;
            set => RaisePropertyChanged(ref _listOfHandShake, value);
        }

        private List<ushort> _listOfDataBits;
        public List<ushort> ListOfDataBits
        {
            get => _listOfDataBits;
            set => RaisePropertyChanged(ref _listOfDataBits, value);
        }
        #endregion

        public SettingsViewModel(ILoaderService loaderService, ILocalAppDataService localAppDataService)
        {
            _loaderService = loaderService;
            _localAppDataService = localAppDataService;

            localDataDevice = new DeviceModel();
            selectDevice = new DeviceModel();

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

            mapDeviceWatchersToDeviceSelector = new Dictionary<DeviceWatcher, string>();
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
            bool hasContainer = _localAppDataService.ContainerLocalAppDataExists(LocalDataType.DeviceData);
            if (hasContainer)
            {
                ApplicationDataCompositeValue compositeValues = _localAppDataService.GetContainerLocalAppData(LocalDataType.DeviceData);

                localDataDevice.Name = (string)compositeValues["DeviceName"];
                localDataDevice.Parity = (string)compositeValues["Parity"];
                localDataDevice.StopBit = (string)compositeValues["StopBit"];
                localDataDevice.HandShake = (string)compositeValues["HandShake"];
                localDataDevice.DataBits = (ushort)compositeValues["DataBits"];
                localDataDevice.BaudRate = (uint)compositeValues["BaudRate"];
            }
            else
            {
                _localAppDataService.CreateContainerLocalAppData(LocalDataType.DeviceData);
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue
                {
                    ["DeviceName"] = "",
                    ["DeviceId"] = "",
                    ["DeviceSelector"] = "",
                    ["Parity"] = "Even",
                    ["StopBit"] = "One",
                    ["HandShake"] = "None",
                    ["DataBits"] = (ushort)8,
                    ["BaudRate"] = (uint)115200,
                };
                _localAppDataService.SetCompositeLocalAppData(LocalDataType.DeviceData, composite);
                localDataDevice.Name = "";
                localDataDevice.Parity = "Even";
                localDataDevice.StopBit = "One";
                localDataDevice.HandShake = "None";
                localDataDevice.DataBits = 8;
                localDataDevice.BaudRate = 115200;
            }
        }

        private void SaveDeviceData()
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            if (selectDevice.Name != null)
            {
                composite["DeviceName"] = selectDevice.Name;
            }
            else
            {
                composite["DeviceName"] = localDataDevice.Name;
            }

            if (selectDevice.Parity != null)
            {
                composite["Parity"] = selectDevice.Parity;
            }
            else
            {
                composite["Parity"] = localDataDevice.Parity;
            }

            if (selectDevice.StopBit != null)
            {
                composite["StopBit"] = selectDevice.StopBit;
            }
            else
            {
                composite["StopBit"] = localDataDevice.StopBit;
            }

            if (selectDevice.HandShake != null)
            {
                composite["HandShake"] = selectDevice.HandShake;
            }
            else
            {
                composite["HandShake"] = localDataDevice.HandShake;
            }

            if (selectDevice.DataBits != 0)
            {
                composite["DataBits"] = selectDevice.DataBits;
            }
            else
            {
                composite["DataBits"] = localDataDevice.DataBits;
            }

            if (selectDevice.BaudRate != 0)
            {
                composite["BaudRate"] = selectDevice.BaudRate;
            }
            else
            {
                composite["BaudRate"] = localDataDevice.BaudRate;
            }

            _localAppDataService.SetCompositeLocalAppData(LocalDataType.DeviceData, composite);
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
