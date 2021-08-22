
namespace Corolla_GUIMVVM_E120.Models
{
    public class ClimateControlModel
    {
        public string AmbientTemperature { get; set; }
        public string InsideTemperature { get; set; }
        public string InsideHumidity { get; set; }
        public string EvaporatorTemperature { get; set; }
        public string AmbientLight { get; set; }
        public string DewPointValue { get; set; }
        public string ThermalSensation { get; set; }
        public string StatusBlower { get; set; }

        public bool Illumination { get; set; }
        public bool StatusFan1 { get; set; }
        public bool StatusMagneticClutch { get; set; }

        public ClimateControlModel()
        {
            AmbientTemperature = "0";
            InsideTemperature = "0";
            InsideHumidity = "0";
            EvaporatorTemperature = "0";
            AmbientLight = "0";
            DewPointValue = "0";
            ThermalSensation = "0";
            StatusBlower = "0";

            Illumination = false;
            StatusFan1 = false;
            StatusMagneticClutch = false;
        }
    }
}
