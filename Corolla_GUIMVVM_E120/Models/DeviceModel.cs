using Corolla_GUIMVVM_E120.ViewModels.Base;

namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceModel : ViewModelBase
    {
        #region Variables privadas
        private string _name;
        private uint _baudRate;
        private string _parity;
        private string _stopBit;
        private string _handShake;
        private ushort _dataBits;
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
        }
        #endregion

        #region Propiedades
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
        #endregion

    }
}
