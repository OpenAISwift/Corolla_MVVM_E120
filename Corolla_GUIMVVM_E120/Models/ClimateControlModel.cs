using Corolla_GUIMVVM_E120.ViewModels.Base;

using System;

namespace Corolla_GUIMVVM_E120.Models
{
    public class ClimateControlModel : ViewModelBase
    {
        #region Variables Privadas

        private string _ambientTemperature = "0";
        private string _insideTemperature = "0";
        private string _insideHumidity = "0";
        private string _evaporatorTemperature = "0";
        private string _ambientLight = "0";
        private string _dewPointValue = "0";
        private string _thermalSensation = "0";

        private bool _enabledWarningLigth = true;
        private bool _enabledFan1 = false;
        private bool _enableDefrosterFront = true;
        private bool _enableDefrostRear = true;
        private bool _enableAc = true;
        private bool _enableAuto = true;
        private bool _enableAirMode = true;
        private bool _enableAirFront = true;
        private bool _enableAirDown = true;
        private bool _enableVentFront = true;
        private bool _enableBlowerUp = true;
        private bool _enableBlowerDown = true;
        private bool _enableAirMixUp = true;
        private bool _enableAirMixDown = true;

        private bool _illumination = false;
        private bool _chekedWarningLigth = false;
        private bool _chekedFan1 = false;
        private bool _chekedDefrosterFront = false;
        private bool _chekedDefrostRear = false;
        private bool _chekedAc = false;
        private bool _chekedAuto = false;
        private bool _chekedAirMode = false;
        private bool _chekedAirFront = false;
        private bool _chekedAirDown = false;
        private bool _chekedVentFront = false;
        #endregion

        #region Propiedades

        public string AmbientTemperature
        {
            get => _ambientTemperature;
            set => RaisePropertyChanged(ref _ambientTemperature, value);
        }
        public string InsideTemperature
        {
            get => _insideTemperature;
            set => RaisePropertyChanged(ref _insideTemperature, value);
        }
        public string InsideHumidity
        {
            get => _insideHumidity;
            set => RaisePropertyChanged(ref _insideHumidity, value);
        }
        public string EvaporatorTemperature
        {
            get => _evaporatorTemperature;
            set => RaisePropertyChanged(ref _evaporatorTemperature, value);
        }
        public string AmbientLight
        {
            get => _ambientLight;
            set => RaisePropertyChanged(ref _ambientLight, value);
        }
        public string DewPointValue
        {
            get => _dewPointValue;
            set => RaisePropertyChanged(ref _dewPointValue, value);
        }
        public string ThermalSensation
        {
            get => _thermalSensation;
            set => RaisePropertyChanged(ref _thermalSensation, value);
        }

        public bool EnableWarningLigth
        {
            get => _enabledWarningLigth;
            set => RaisePropertyChanged(ref _enabledWarningLigth, value);
        }
        public bool EnabledDefrosterFront
        {
            get => _enableDefrosterFront;
            set => RaisePropertyChanged(ref _enableDefrosterFront, value);
        }
        public bool EnabledDefrostRear
        {
            get => _enableDefrostRear;
            set => RaisePropertyChanged(ref _enableDefrostRear, value);
        }
        public bool EnabledAc
        {
            get => _enableAc;
            set => RaisePropertyChanged(ref _enableAc, value);
        }
        public bool EnabledAuto
        {
            get => _enableAuto;
            set => RaisePropertyChanged(ref _enableAuto, value);
        }
        public bool EnabledAirMode
        {
            get => _enableAirMode;
            set => RaisePropertyChanged(ref _enableAirMode, value);
        }
        public bool EnabledFan1
        {
            get => _enabledFan1;
            set => RaisePropertyChanged(ref _enabledFan1, value);
        }
        public bool EnabledAirFront
        {
            get => _enableAirFront;
            set => RaisePropertyChanged(ref _enableAirFront, value);
        }
        public bool EnabledAirDown
        {
            get => _enableAirDown;
            set => RaisePropertyChanged(ref _enableAirDown, value);
        }
        public bool EnabledVentFront
        {
            get => _enableVentFront;
            set => RaisePropertyChanged(ref _enableVentFront, value);
        }
        public bool EnableBlowerUp
        {
            get => _enableBlowerUp;
            set => RaisePropertyChanged(ref _enableBlowerUp, value);
        }
        public bool EnableBlowerDown
        {
            get => _enableBlowerDown;
            set => RaisePropertyChanged(ref _enableBlowerDown, value);
        }
        public bool EnableAirMixUp
        {
            get => _enableAirMixUp;
            set => RaisePropertyChanged(ref _enableAirMixUp, value);
        }
        public bool EnableAirMixDown
        {
            get => _enableAirMixDown;
            set => RaisePropertyChanged(ref _enableAirMixDown, value);
        }

        public bool Illumination
        {
            get => _illumination;
            set => RaisePropertyChanged(ref _illumination, value);
        }
        public bool ChekedWarningLigth
        {
            get => _chekedWarningLigth;
            set => RaisePropertyChanged(ref _chekedWarningLigth, value);
        }
        public bool ChekedDefrosterFront
        {
            get { return _chekedDefrosterFront; }
            set { RaisePropertyChanged(ref _chekedDefrosterFront, value); }
        }
        public bool ChekedDefrostRear
        {
            get { return _chekedDefrostRear; }
            set { RaisePropertyChanged(ref _chekedDefrostRear, value); }
        }
        public bool ChekedAc
        {
            get { return _chekedAc; }
            set { RaisePropertyChanged(ref _chekedAc, value); }
        }
        public bool ChekedAuto
        {
            get { return _chekedAuto; }
            set { RaisePropertyChanged(ref _chekedAuto, value); }
        }
        public bool ChekedAirMode
        {
            get { return _chekedAirMode; }
            set { RaisePropertyChanged(ref _chekedAirMode, value); }
        }
        public bool ChekedFan1
        {
            get { return _chekedFan1; }
            set { RaisePropertyChanged(ref _chekedFan1, value); }
        }
        public bool ChekedAirFront
        {
            get { return _chekedAirFront; }
            set { RaisePropertyChanged(ref _chekedAirFront, value); }
        }
        public bool ChekedAirDown
        {
            get { return _chekedAirDown; }
            set { RaisePropertyChanged(ref _chekedAirDown, value); }
        }
        public bool ChekedVentFront
        {
            get { return _chekedVentFront; }
            set { RaisePropertyChanged(ref _chekedVentFront, value); }
        }
        #endregion

        #region Comandos

        #endregion

        #region Metodos

        #endregion
    }
}
