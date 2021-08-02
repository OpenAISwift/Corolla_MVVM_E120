using Corolla_GUIMVVM_E120.Views.Base;
using Corolla_GUIMVVM_E120.ViewModels;

namespace Corolla_GUIMVVM_E120.Views
{
    public sealed partial class SettingsView : PageBase
    {
        private SettingsViewModel Settings => (SettingsViewModel)DataContext;

        public SettingsView()
        {
            this.InitializeComponent();
        }
    }
}
