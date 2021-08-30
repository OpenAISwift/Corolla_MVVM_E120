using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceNewDataEventArgs : EventArgs
    {
        public string AmbientTemperature { get; set; }
        public string InsideTemperature { get; set; }
        public string InsideHumidity { get; set; }
        public string EvaporatorTemperature { get; set; }
        public string AmbientLight { get; set; }
        public string DewPoint { get; set; }
        public string ThermalSensation { get; set; }

    }
}
