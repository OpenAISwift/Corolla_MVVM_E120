using Corolla_GUIMVVM_E120.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corolla_GUIMVVM_E120.Models
{
    public class DeviceModel : ViewModelBase
    {
        private string _deviceName = null;
        public string DeviceName
        {
            get => _deviceName;
            set => RaisePropertyChanged(ref _deviceName, value);
        }
    }
}
