using Corolla_GUIMVVM_E120.ViewModels.Base;
using System;

namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceModel : IEquatable<DeviceModel>
    {
        #region Propiedades
        public uint BaudRate { get; set; }

        public ushort DataBits { get; set; }

        public bool AutoReconect { get; set; }
        public bool BreakSignalState { get; set; }
        public bool DataTerminalReadyEnabled { get; set; }
        public bool RequestToSendEnabled { get; set; }

        public string Name { get; set; }
        public string Parity { get; set; }
        public string StopBit { get; set; }
        public string HandShake { get; set; }
        #endregion

        #region Metodos

        public DeviceModel()
        {
            Name = "";
            Parity = "None";
            StopBit = "One";
            HandShake = "None";

            BaudRate = 115200;
            DataBits = 8;

            AutoReconect = true;
            BreakSignalState = false;
            DataTerminalReadyEnabled = true;
            RequestToSendEnabled = true;
        }

        public override bool Equals(object obj)
        {
            return obj is DeviceModel _device && Equals(_device);
        }

        public override int GetHashCode()
        {
            return DataBits.GetHashCode() ^ Name.GetHashCode();
        }

        public bool Equals(DeviceModel _deviceModel)
        {
            //Es la misma instancia
            if (ReferenceEquals(this, _deviceModel))
            {
                return true;
            }

            // Realiza comparaciones
            if (Name.CompareTo(_deviceModel.Name) != 0)
            {
                return true;
            }
            if (BaudRate.CompareTo(_deviceModel.BaudRate) != 0)
            {
                return true;
            }
            if (Parity.CompareTo(_deviceModel.Parity) != 0)
            {
                return true;
            }
            if (StopBit.CompareTo(_deviceModel.StopBit) != 0)
            {
                return true;
            }
            if (HandShake.CompareTo(_deviceModel.HandShake) != 0)
            {
                return true;
            }
            if (DataBits.CompareTo(_deviceModel.DataBits) != 0)
            {
                return true;
            }
            if (AutoReconect.CompareTo(_deviceModel.AutoReconect) != 0)
            {
                return true;
            }
            if (BreakSignalState.CompareTo(_deviceModel.BreakSignalState) != 0)
            {
                return true;
            }
            if (DataTerminalReadyEnabled.CompareTo(_deviceModel.DataTerminalReadyEnabled) != 0)
            {
                return true;
            }
            if (RequestToSendEnabled.CompareTo(_deviceModel.RequestToSendEnabled) != 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
