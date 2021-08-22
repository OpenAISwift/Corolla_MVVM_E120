﻿using Corolla_GUIMVVM_E120.Models;
using System;

namespace Corolla_GUIMVVM_E120.Services.SerialDeviceService
{
    public interface ISerialDeviceService
    {
        /// <summary>
        /// Comprueba la conexión con el dispositivo serial 
        /// </summary>
        /// <param name="device"></param>
        void DeviceCheck(DeviceModel device);
        
        ClimateControlModel ControlModelData { get; }

        event EventHandler AvailableData;
        void WriteToPort(string DataDevice);
    }
}
