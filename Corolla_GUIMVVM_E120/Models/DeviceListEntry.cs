using Corolla_GUIMVVM_E120.Contants;
using System;
using Windows.Devices.Enumeration;

namespace Corolla_GUIMVVM_E120.Models
{
    /// <summary>
    /// La clase sólo expondrá las propiedades de DeviceInformation que se van a utilizar
    /// en este ejemplo. Cada instancia de esta clase proporciona información sobre un único dispositivo.
    ///
    /// Esta clase es utilizada por la UI para mostrar información específica del dispositivo para que
    /// el usuario pueda identificar qué dispositivo utilizar.
    /// <summary>
    public class DeviceListEntry
    {
        private DeviceInformation device = null;
        private string deviceSelector = null;

        public string DeviceName => device.Name;
        public string DeviceId => device.Id;

        public string InstanceId => device.Properties[DeviceProperties.DeviceInstanceId] as string;
        public DeviceInformation DeviceInformation => device;
        public string DeviceSelector => deviceSelector;

        /// <summary>
        /// La clase se utiliza principalmente como una envoltura de DeviceInformation para que la UI pueda vincularse a una lista de estos.
        /// <summary>
        /// <param name="deviceInformation"></param>
        /// <param name="deviceSelector">El AQS utilizado para encontrar este dispositivo</param>
        public DeviceListEntry(DeviceInformation deviceInformation, string deviceSelector)
        {
            device = deviceInformation;
            this.deviceSelector = deviceSelector;
        }
    }
}
