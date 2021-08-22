using Corolla_GUIMVVM_E120.ViewModels;
using Corolla_GUIMVVM_E120.Views.Base;

namespace Corolla_GUIMVVM_E120.Views
{
    public sealed partial class InfoView : PageBase
    {
        private InfoViewModel Info => (InfoViewModel)DataContext;

        public InfoView()
        {
            InitializeComponent();
        }
    }
}
