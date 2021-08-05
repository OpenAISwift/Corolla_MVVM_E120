using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Corolla_GUIMVVM_E120.Views.Base;
using Corolla_GUIMVVM_E120.ViewModels;

namespace Corolla_GUIMVVM_E120.Views
{
    public sealed partial class ClimateControlView : PageBase
    {
        private ClimateControlViewModel ClimateControl => (ClimateControlViewModel)DataContext;

        public ClimateControlView()
        {
            this.InitializeComponent();
        }
    }
}
