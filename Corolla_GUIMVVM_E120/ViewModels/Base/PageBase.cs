using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Corolla_GUIMVVM_E120.ViewModels.Base
{
    public abstract class ViewModelPageBase : ViewModelBase
    {
        private Frame appFrame;
        public Frame AppFrame
        {
            get { return appFrame; }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => RaisePropertyChanged(ref isBusy, value);
        }

        public abstract Task OnNavigatedFrom(NavigationEventArgs args);
        public abstract Task OnNavigatedTo(NavigationEventArgs args);

        internal void SetAppFrame(Frame viewFrame)
        {
            appFrame = viewFrame;
        }
    }
}
