using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corolla_GUIMVVM_E120.Contants
{
    public enum NotifyType
    {
        Status,
        Information,
        Error
    };

    public enum LocalDataType
    {
        DeviceData,
        AppData
    }

    public class DeviceProperties
    {
        public const string DeviceInstanceId = "System.Devices.DeviceInstanceId";
    }
}
