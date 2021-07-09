using Corolla_GUIMVVM_E120.Contants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corolla_GUIMVVM_E120.Services.DialogService
{
    public interface IDialogService
    {
        void StatusNotification(string message, NotifyType type);
    }
}
