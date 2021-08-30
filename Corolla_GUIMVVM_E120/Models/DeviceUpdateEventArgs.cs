using System;

namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceUpdateEventArgs : EventArgs
    {
        public byte BlowerStatus { get; set; }
        public byte AirMixStatus { get; set; }

        public bool IlluminationStatus { get; set; }
        public bool FanSatus { get; set; }
        public bool MagneticClutchStatus { get; set; }

        public DeviceUpdateEventArgs()
        {
            BlowerStatus = 0;
            AirMixStatus = 0;

            IlluminationStatus = false;
            FanSatus = false;
            MagneticClutchStatus = false;
        }

    }
}
