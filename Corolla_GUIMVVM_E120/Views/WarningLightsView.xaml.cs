using Corolla_GUIMVVM_E120.ViewModels;
using Corolla_GUIMVVM_E120.Views.Base;


namespace Corolla_GUIMVVM_E120.Views
{
    public sealed partial class WarningLightsView : PageBase
    {
        private WarningLightsViewModel Control => (WarningLightsViewModel)DataContext;

        public WarningLightsView()
        {
            this.InitializeComponent();
        }
    }
}
