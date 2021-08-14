using Corolla_GUIMVVM_E120.ViewModels.Base;
using System;

namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceModel : ViewModelBase, IEquatable<DeviceModel>
    {
        #region Variables privadas
        private int _iD;
        private string _name;
        private uint _baudRate;
        private string _parity;
        private string _stopBit;
        private string _handShake;
        private ushort _dataBits;

        private bool _autoReconect;
        #endregion

        #region Metodos
        public DeviceModel()
        {
            Name = null;
            BaudRate = 0;
            Parity = null;
            StopBit = null;
            HandShake = null;
            DataBits = 0;
            AutoReconect = true;
        }
        #endregion

        #region Propiedades
        public int ID
        {
            get => _iD;
            set => RaisePropertyChanged(ref _iD, value);
        }
        public string Name
        {
            get => _name;
            set => RaisePropertyChanged(ref _name, value);
        }
        public uint BaudRate
        {
            get => _baudRate;
            set => RaisePropertyChanged(ref _baudRate, value);
        }
        public string Parity
        {
            get => _parity;
            set => RaisePropertyChanged(ref _parity, value);
        }
        public string StopBit
        {
            get => _stopBit;
            set => RaisePropertyChanged(ref _stopBit, value);

        }
        public string HandShake
        {
            get => _handShake;
            set => RaisePropertyChanged(ref _handShake, value);
        }
        public ushort DataBits
        {
            get => _dataBits;
            set => RaisePropertyChanged(ref _dataBits, value);
        }

        public bool AutoReconect
        {
            get => _autoReconect;
            set => RaisePropertyChanged(ref _autoReconect, value);
        }
        #endregion

        #region Metodos
        public override bool Equals(object obj)
        {
            if (!(obj is DeviceModel _device))
            {
                return false;
            }
            else
            {
                return Equals(_device);
            }
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode()
                ^ this.Name.GetHashCode();
        }

        public bool Equals(DeviceModel _deviceModel )
        {
            //Es la misma instancia
            if (ReferenceEquals(this,_deviceModel))
            {
                return true;
            }

            // Realiza comparaciones

            //if (this.Name.CompareTo(_deviceModel.Name)!=0)
            //{
            //    return false;
            //}

            if (this.BaudRate.CompareTo(_deviceModel.BaudRate) != 0)
            {
                return true;
            }
            if (this.Parity.CompareTo(_deviceModel.Parity) != 0)
            {
                return true;
            }
            if (this.StopBit.CompareTo(_deviceModel.StopBit) != 0)
            {
                return true;
            }
            if (this.HandShake.CompareTo(_deviceModel.HandShake) != 0)
            {
                return true;
            }
            if (this.DataBits.CompareTo(_deviceModel.DataBits) != 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
