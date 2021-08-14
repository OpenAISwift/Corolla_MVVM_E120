using Corolla_GUIMVVM_E120.Models;
using System;

namespace Corolla_GUIMVVM_E120.Services.SerialDeviceService
{
    public interface ISerialDeviceService
    {
        void InitDeviceAsync(DeviceModel device);
        event EventHandler NewDataDevice; 
    }
}
