using System;

namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceStatusEventArgs : EventArgs
    {
        public bool IsDeviceConnected { get; set; }
        public string DeviceMessage { get; set; }
    }
}
