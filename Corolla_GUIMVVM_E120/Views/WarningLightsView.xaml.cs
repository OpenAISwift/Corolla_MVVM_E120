using Corolla_GUIMVVM_E120.ViewModels;
using Corolla_GUIMVVM_E120.Views.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Corolla_GUIMVVM_E120.Views
{
    public sealed partial class WarningLightsView : PageBase
    {
        private ClimateControlViewModel ClimateControl => (ClimateControlViewModel)DataContext;

        public WarningLightsView()
        {
            this.InitializeComponent();
        }
    }
}
