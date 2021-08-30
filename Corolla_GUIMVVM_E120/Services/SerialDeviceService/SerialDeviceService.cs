using Corolla_GUIMVVM_E120.Models;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace Corolla_GUIMVVM_E120.Services.SerialDeviceService
{
    public class SerialDeviceService : ISerialDeviceService
    {
        #region Propiedades

        #region Declaracion de Eventos
        //private SuspendingEventHandler appSuspendEventHandler;
        //private EventHandler<object> appResumeEventHandler;

        private TypedEventHandler<SerialDeviceService, DeviceInformation> deviceCloseCallback;
        private TypedEventHandler<SerialDeviceService, DeviceInformation> deviceConnectedCallback;

        private TypedEventHandler<DeviceWatcher, DeviceInformation> deviceAddedEventHandler;
        private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> deviceRemovedEventHandler;
        private TypedEventHandler<DeviceAccessInformation, DeviceAccessChangedEventArgs> deviceAccessEventHandler;

        public event EventHandler<DeviceNewDataEventArgs> EventDeviceNewData;
        public event EventHandler<DeviceUpdateEventArgs> EventDeviceUpdate;

        public event EventHandler<DeviceStatusEventArgs> EventDeviceStatus;
        #endregion

        #region Declaracion de clases

        private DeviceModel localDeviceModel;
        private DeviceInformation deviceInformation;
        private DeviceAccessInformation deviceAccessInformation;
        private DeviceWatcher deviceWatcher;

        private SerialDevice device;

        /// Read Operation
        private CancellationTokenSource ReadCancellationTokenSource;
        private readonly object ReadCancelLock = new object();
        private DataReader DataReaderObject = null;

        // Write Operation
        private CancellationTokenSource WriteCancellationTokenSource;
        private readonly object WriteCancelLock = new object();
        private DataWriter DataWriteObject = null;
        #endregion

        /// <summary>
        /// DeviceSelector AQS es utilizado para encontrar este dispositivo
        /// </summary>
        private string deviceSelector;

        private readonly string[] str_Delimiter = new string[] { " ", ",", ":" };
        private string[] str_data;

        private bool changedSettings;
        private bool deviceIsDetected;
        //private bool watcherSuspended;
        private bool watcherStarted;
        private bool isEnabledAutoReconnect;
        #endregion

        #region Propiedades
        private bool IsDeviceConnected => device != null;

        public ClimateControlModel ControlModelData { get; }

        #endregion

        #region Metodos

        public SerialDeviceService()
        {
            localDeviceModel = new DeviceModel();
            ControlModelData = new ClimateControlModel();

            watcherStarted = false;
            //watcherSuspended = false;

            deviceNewDataEventArgs = new DeviceNewDataEventArgs();
        }

        #region Metodos de Eventos
        private DeviceNewDataEventArgs deviceNewDataEventArgs;
        /// <summary>
        /// Registro para los eventos Added y Removed.
        /// Tenga en cuenta que, al desconectar el dispositivo, éste puede ser cerrado por el sistema
        /// antes de que se invoque la devolución de llamada OnDeviceRemoved.
        /// <summary>
        private void RegisterForDeviceWatcherEvents()
        {
            deviceAddedEventHandler = new TypedEventHandler<DeviceWatcher, DeviceInformation>(OnDeviceAdded);
            deviceRemovedEventHandler = new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(OnDeviceRemoved);

            deviceWatcher.Added += deviceAddedEventHandler;
            deviceWatcher.Removed += deviceRemovedEventHandler;
        }

        private void UnregisterFromDeviceAccessStatusChange()
        {
            deviceAccessInformation.AccessChanged -= deviceAccessEventHandler;

            deviceAccessEventHandler = null;
        }

        /// <summary>
        /// Registro de eventos de suspensión/reanudación de la aplicación. 
        /// También nos registraremos para cuando la app exista 
        /// </summary>
        //private void RegisterForAppEvents()
        //{
        //    appSuspendEventHandler = new SuspendingEventHandler(OnAppSuspension);
        //    appResumeEventHandler = new EventHandler<object>(OnAppResume);

        //    // Este evento se lanza cuando se sale de la aplicación
        //    // y cuando se suspende la aplicación
        //    Application.Current.Suspending += appSuspendEventHandler;
        //    Application.Current.Resuming += appResumeEventHandler;
        //}

        /// <summary>
        /// Este evento se lanza cuando se sale de la aplicación y 
        /// cuando se suspende la aplicación
        /// </summary>
        //private void UnregisterFromAppEvents()
        //{
        //    Application.Current.Suspending -= appSuspendEventHandler;
        //    Application.Current.Resuming -= appResumeEventHandler;

        //    appSuspendEventHandler = null;
        //    appResumeEventHandler = null;
        //}

        protected virtual void DeviceStatusEvent(DeviceStatusEventArgs eventArgs)
        {
            EventDeviceStatus?.Invoke(this, eventArgs);
        }

        protected virtual void DeviceNewDataEvent(DeviceNewDataEventArgs eventArgs)
        {
            EventDeviceNewData?.Invoke(this, eventArgs);
        }

        protected virtual void DeviceUpdateEvent(DeviceUpdateEventArgs eventArgs)
        {
            EventDeviceUpdate?.Invoke(this, eventArgs);
        }

        #endregion

        /// <summary>
        /// Si se ha instanciado un objeto Serial (se abre un handle del dispositivo), 
        /// debemos cerrarlo antes de que la app entre en suspensión porque la API lo 
        /// cierra automáticamente por nosotros si no lo hacemos. Al reanudar, la API
        /// no reabrirá el dispositivo automáticamente, por lo que debemos abrirlo 
        /// explícitamente en la aplicación.
        ///
        /// Dado que tenemos que reabrir el dispositivo nosotros mismos cuando la 
        /// aplicación se reanuda, es una buena práctica llamar explícitamente a la 
        /// función de cierre en la aplicación (Para cada apertura hay un cierre).
        /// 
        /// Debemos detener el DeviceWatcher porque seguirá lanzando eventos aunque
        /// la app está en suspensión, lo cual no es deseado (drena la batería). 
        /// Reanudamos el device watcher una vez que la app se reanude de nuevo.
        /// <summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        //private void OnAppSuspension(object sender, SuspendingEventArgs args)
        //{
        //    if (watcherStarted)
        //    {
        //        watcherSuspended = true;
        //        StopDeviceWatcher();
        //    }
        //    else
        //    {
        //        watcherSuspended = false;
        //    }

        //    CloseCurrentlyConnectedDevice();
        //}

        /// <summary>
        /// Al reanudar en la aplicación, debemos reabrir un handle al dispositivo 
        /// Serial de nuevo. Esto ocurrirá automáticamente ocurrirá cuando iniciemos 
        /// de nuevo el observador de dispositivos; el dispositivo se volverá a 
        /// enumerar e intentaremos reabrirlo si la propiedad IsEnabledAutoReconnect 
        /// está activada.
        /// 
        /// Ver OnAppSuspension para saber por qué estamos iniciando el observador de 
        /// dispositivos de nuevo.
        /// <summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        //private void OnAppResume(object sender, object args)
        //{
        //    if (watcherSuspended)
        //    {
        //        watcherSuspended = false;
        //        StartDeviceWatcher();
        //    }
        //}

        public void DeviceCheck(DeviceModel device)
        {
            if (device != null)
            {
                if (IsDeviceConnected)
                {
                    if (device.Name != localDeviceModel.Name)
                    {
                        CloseDevice();
                    }
                    else
                    {
                        if (localDeviceModel.Equals(device))
                        {
                            changedSettings = true;
                        }
                    }
                }
                else
                {
                    DeviceCheck();
                }
                localDeviceModel = device;
                if (changedSettings)
                {
                    UpdateDeviceParameters();

                    changedSettings = false;
                }

            }
        }

        private async void DeviceCheck()
        {
            deviceIsDetected = await FindLocalDevice();

            if (IsDeviceConnected != true && deviceIsDetected)
            {
                _ = await OpenDeviceAsync();
            }
            else
            {
                DeviceStatusEventArgs data = new DeviceStatusEventArgs
                {
                    IsDeviceConnected = false,
                    DeviceMessage = "Dispositivo no encontrado"
                };

                DeviceStatusEvent(data);
            }
        }

        private async Task<bool> FindLocalDevice()
        {
            if (localDeviceModel.Name != null)
            {
                deviceSelector = SerialDevice.GetDeviceSelector();
                DeviceInformationCollection list = await DeviceInformation.FindAllAsync(deviceSelector);
                foreach (DeviceInformation item in list)
                {
                    if (item.Name == localDeviceModel.Name)
                    {
                        deviceInformation = item;
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Este método abre el dispositivo utilizando la API Serial de WinRT. 
        /// Después de abrir el dispositivo, guarda el dispositivo para que pueda ser utilizado en otros escenarios.
        /// Es importante que la llamada FromIdAsync se haga en el hilo de la UI porque el
        /// prompt de consentimiento sólo puede ser mostrado en el hilo de la interfaz de usuario.
        /// Este método se utiliza para reabrir el dispositivo después de que el dispositivo se
        /// reconecta al ordenador y cuando la aplicación se reanuda.
        /// </summary>
        public async Task<bool> OpenDeviceAsync()
        {
            device = await SerialDevice.FromIdAsync(deviceInformation.Id);

            bool successfullyOpenedDevice = false;

            if (device != null)
            {
                UpdateDeviceParameters();

                //Notificar a la llamada de retorno registrada que el dispositivo ha sido abierto
                deviceConnectedCallback?.Invoke(this, deviceInformation);
                //if (appSuspendEventHandler == null || appResumeEventHandler == null)
                //{
                //    RegisterForAppEvents();
                //}
                // Crear y registrar los eventos del observador de dispositivos para el dispositivo
                // que se va a abrir a menos que estemos reabriendo el dispositivo
                if (deviceWatcher == null)
                {
                    deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);
                    RegisterForDeviceWatcherEvents();
                }
                if (!watcherStarted)
                {
                    // Inicie el observador de dispositivos después de que nos hayamos asegurado
                    // de que el dispositivo está abierto.
                    StartDeviceWatcher();
                }
                if (IsDeviceConnected)
                {
                    ReadDataPort();
                }

                successfullyOpenedDevice = true;
                DeviceStatusEventArgs data = new DeviceStatusEventArgs
                {
                    IsDeviceConnected = true,
                    DeviceMessage = null
                };
                DeviceStatusEvent(data);
            }
            else
            {
                DeviceStatusEventArgs data = new DeviceStatusEventArgs
                {
                    IsDeviceConnected = false,
                };
                DeviceAccessStatus deviceAccessStatus = DeviceAccessInformation.CreateFromId(deviceInformation.Id).CurrentStatus;

                data.DeviceMessage = deviceAccessStatus == DeviceAccessStatus.DeniedByUser
                    ? "Acceso al dispositivo bloquedo por el usuario"
                    : deviceAccessStatus == DeviceAccessStatus.DeniedBySystem
                    ? "Acceso al dispositivo bloquedo por el sistema"
                    : "Error desconocido, posiblemente abierto por otra aplicación";

                successfullyOpenedDevice = false;
                DeviceStatusEvent(data);
            }
            return successfullyOpenedDevice;
        }

        private void UpdateDeviceParameters()
        {
            device.ReadTimeout = TimeSpan.FromMilliseconds(100);
            device.WriteTimeout = TimeSpan.FromMilliseconds(100);

            device.BaudRate = localDeviceModel.BaudRate;
            device.DataBits = localDeviceModel.DataBits;
            device.Parity = (SerialParity)Enum.Parse(typeof(SerialParity), localDeviceModel.Parity);
            device.StopBits = (SerialStopBitCount)Enum.Parse(typeof(SerialStopBitCount), localDeviceModel.StopBit);
            device.Handshake = (SerialHandshake)Enum.Parse(typeof(SerialHandshake), localDeviceModel.HandShake);
            device.DataBits = localDeviceModel.DataBits;
            isEnabledAutoReconnect = localDeviceModel.AutoReconect;
            device.IsDataTerminalReadyEnabled = localDeviceModel.DataTerminalReadyEnabled;
            device.IsRequestToSendEnabled = localDeviceModel.RequestToSendEnabled;
        }

        private async void ReadDataPort()
        {
            ReadCancellationTokenSource = new CancellationTokenSource();
            WriteCancellationTokenSource = new CancellationTokenSource();
            while (!ReadCancellationTokenSource.IsCancellationRequested)
            {
                await Listen();
            }
        }

        private async Task Listen()
        {
            if (IsDeviceConnected)
            {
                try
                {
                    DataReaderObject = new DataReader(device.InputStream);
                    await ReadAsync(ReadCancellationTokenSource.Token);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                }
                finally
                {
                    if (DataReaderObject != null)
                    {
                        _ = DataReaderObject.DetachStream();
                        DataReaderObject = null;
                    }
                }
            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<uint> loadAsyncTask;
            uint ReadBufferLength = 256;
            DataReaderObject.InputStreamOptions = InputStreamOptions.None;

            loadAsyncTask = DataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);
            uint bytesRead = await loadAsyncTask;

            if (bytesRead > 0)
            {
                string strFromPort = DataReaderObject.ReadString(bytesRead);
                FrameAnalyzer(strFromPort);
                Debug.WriteLine("Recivido:" + strFromPort);
            }
        }

        private void FrameAnalyzer(string data)
        {
            bool updateData = false;
            bool updateStatus = false;

            string str_dataInt = data;

            DeviceUpdateEventArgs updateEventArgs = new DeviceUpdateEventArgs();

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
                                updateEventArgs.IlluminationStatus = true;
                                updateStatus = true;
                                break;
                            }
                            else
                            {
                                updateEventArgs.IlluminationStatus = false;
                                updateStatus = true;
                                break;
                            }
                        case "Te":
                            deviceNewDataEventArgs.EvaporatorTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            updateData = true;
                            break;
                        case "Ta":
                            deviceNewDataEventArgs.AmbientTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            updateData = true;
                            break;
                        case "Ti":
                            deviceNewDataEventArgs.InsideTemperature = Convert.ToString(short.Parse(str_data[i + 1]));
                            updateData = true;
                            break;
                        case "Ts":
                            deviceNewDataEventArgs.ThermalSensation = Convert.ToString(short.Parse(str_data[i + 1]));
                            updateData = true;
                            break;
                        case "H":
                            deviceNewDataEventArgs.InsideHumidity = Convert.ToString(short.Parse(str_data[i + 1]));
                            updateData = true;
                            break;
                        case "Pr":
                            deviceNewDataEventArgs.DewPoint = Convert.ToString(short.Parse(str_data[i + 1]));
                            updateData = true;
                            break;
                        case "La":
                            deviceNewDataEventArgs.AmbientLight = Convert.ToString(short.Parse(str_data[i + 1]));
                            updateData = true;
                            break;
                        case "F1":
                            if (str_data[i + 1] == "1")
                            {
                                updateEventArgs.FanSatus = true;
                                updateStatus = true;
                                break;
                            }
                            else
                            {
                                updateEventArgs.FanSatus = false;
                                updateStatus = true;
                                break;
                            }
                        case "Bl":
                            updateEventArgs.BlowerStatus = byte.Parse(str_data[i + 1]);
                            updateStatus = true;
                            break;
                        case "Co":
                            if (str_data[i + 1] == "1")
                            {
                                updateEventArgs.MagneticClutchStatus = true;
                                updateStatus = true;
                                break;
                            }
                            else
                            {
                                updateEventArgs.MagneticClutchStatus = false;
                                updateStatus = true;
                                break;
                            }

                        default:
                            break;
                    }
                }
                if (updateData)
                {
                    DeviceNewDataEvent(deviceNewDataEventArgs);
                }
                if (updateStatus)
                {
                    DeviceUpdateEvent(updateEventArgs);
                }
            }
        }

        public async void WriteToPort(string DataDevice)
        {
            if (IsDeviceConnected)
            {
                try
                {
                    DataWriteObject = new DataWriter(device.OutputStream);
                    await WriteAsync(WriteCancellationTokenSource.Token, DataDevice);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                }
                finally
                {
                    if (DataWriteObject != null)
                    {
                        _ = DataWriteObject.DetachStream();
                        DataWriteObject = null;
                    }
                }
            }
        }

        private async Task WriteAsync(CancellationToken cancellationToken, string dataSentDevice)
        {
            Task<uint> storeAsyncTask;

            if (dataSentDevice.Length != 0)
            {
                Debug.WriteLine("Enviado:" + dataSentDevice);
                _ = DataWriteObject.WriteString(dataSentDevice);
                lock (WriteCancelLock)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // Cancellation Token will be used so we can stop the task operation explicitly
                    // The completion function should still be called so that we can properly handle a canceled task
                    storeAsyncTask = DataWriteObject.StoreAsync().AsTask(cancellationToken);
                }
                _ = await storeAsyncTask;
            }
        }

        /// <summary>
        /// Cierra el dispositivo que se abre para que todas las operaciones pendientes 
        /// se cancelen correctamente.
        /// <summary>
        /// <param name="sender"></param>
        /// <param name="deviceInformationUpdate"></param>
        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInformationUpdate)
        {
            if (IsDeviceConnected && (deviceInformationUpdate.Id == deviceInformation.Id))
            {
                // Las principales razones para cerrar el dispositivo explícitamente es para limpiar los recursos,
                // para manejar adecuadamente los errores y dejar de hablar con el dispositivo desconectado.
                CloseCurrentlyConnectedDevice();
            }
        }

        /// <summary>
        /// Abre el dispositivo que el usuario quería abrir si aún no se ha abierto y la reconexión automática está activada.
        /// <summary>
        /// <param name="sender"></param>
        /// <param name="deviceInfo"></param>
        private async void OnDeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            if ((deviceInformation != null) && (deviceInfo.Id == deviceInformation.Id) && !IsDeviceConnected && isEnabledAutoReconnect)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    _ = await OpenDeviceAsync();
                });
            }
        }

        /// <summary>
        /// Cierra el dispositivo, detiene el observador del dispositivo, 
        /// deja de escuchar los eventos de la aplicación, 
        /// y restablece el estado del objeto a antes de que un dispositivo
        /// estuviera conectado.
        /// </summary>
        public void CloseDevice()
        {
            if (IsDeviceConnected)
            {
                CloseCurrentlyConnectedDevice();
            }

            if (deviceWatcher != null)
            {
                if (watcherStarted)
                {
                    StopDeviceWatcher();

                    UnregisterFromDeviceWatcherEvents();
                }

                deviceWatcher = null;
            }

            if (deviceAccessInformation != null)
            {
                UnregisterFromDeviceAccessStatusChange();

                deviceAccessInformation = null;
            }

            //if (appSuspendEventHandler != null || appResumeEventHandler != null)
            //{
            //    UnregisterFromAppEvents();
            //}

            deviceInformation = null;
            deviceSelector = null;

            deviceConnectedCallback = null;
            deviceCloseCallback = null;
        }

        private void UnregisterFromDeviceWatcherEvents()
        {
            deviceWatcher.Added -= deviceAddedEventHandler;
            deviceAddedEventHandler = null;

            deviceWatcher.Removed -= deviceRemovedEventHandler;
            deviceRemovedEventHandler = null;
        }

        /// <summary>
        /// Este método demuestra cómo cerrar el dispositivo correctamente utilizando 
        /// la API Serial de WinRT.
        /// Cuando el SerialDevice se cierra, cancelará todas las operaciones IO que 
        /// estén pendientes(no completadas).
        /// El cierre no esperará a que se llame a ninguna llamada de finalización de IO,
        /// por lo que la llamada de cierre puede completarse antes de que cualquiera de
        /// las devoluciones de llamada de finalización de IO sean llamadas.
        /// Las operaciones IO pendientes seguirán llamando a sus respectivas devoluciones
        /// de llamada de finalización con un error de tarea error cancelado o la operación 
        /// completada.
        /// </summary>
        private void CloseCurrentlyConnectedDevice()
        {
            CancellAllTasks();
            if (device != null)
            {
                // Notificar al callback que estamos a punto de cerrar el dispositivo
                deviceCloseCallback?.Invoke(this, deviceInformation);

                // Cierra el dispositivo
                device.Dispose();
                device = null;

                _ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                   {
                       DeviceStatusEventArgs data = new DeviceStatusEventArgs
                       {
                           IsDeviceConnected = false,
                           DeviceMessage = "Dispositivo desconectado"
                       };
                       DeviceStatusEvent(data);
                   });
            }
        }

        private void StartDeviceWatcher()
        {
            if ((deviceWatcher.Status != DeviceWatcherStatus.Started)
                && (deviceWatcher.Status != DeviceWatcherStatus.EnumerationCompleted))
            {
                deviceWatcher.Start();
            }

            watcherStarted = true;
        }

        private void StopDeviceWatcher()
        {
            if ((deviceWatcher.Status == DeviceWatcherStatus.Started)
                || (deviceWatcher.Status == DeviceWatcherStatus.EnumerationCompleted))
            {
                deviceWatcher.Stop();
            }

            watcherStarted = false;
        }

        private void CancellAllTasks()
        {
            #region Cancelacion de tarea de lectura
            lock (ReadCancelLock)
            {
                if (ReadCancellationTokenSource != null)
                {
                    if (!ReadCancellationTokenSource.IsCancellationRequested)
                    {
                        ReadCancellationTokenSource.Cancel();
                    }
                }
            }
            #endregion

            #region Cancelacion de tarea de Escritura
            lock (WriteCancelLock)
            {
                if (WriteCancellationTokenSource != null)
                {
                    if (!WriteCancellationTokenSource.IsCancellationRequested)
                    {
                        WriteCancellationTokenSource.Cancel();
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
