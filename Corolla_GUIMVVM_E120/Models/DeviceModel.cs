namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceModel
    {
        public string DeviceName { get; set; }
        public string Parity { get; set; }
        public string StopBit { get; set; }
        public string HandShake { get; set; }
        public ushort DataBits { get; set; }
        public uint BaudRate { get; set; }
    }
}
