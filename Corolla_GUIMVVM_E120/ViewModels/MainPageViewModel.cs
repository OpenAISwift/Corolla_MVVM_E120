using Corolla_GUIMVVM_E120.Services.DialogService;
using Corolla_GUIMVVM_E120.Services.LoaderService;
using Corolla_GUIMVVM_E120.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Corolla_GUIMVVM_E120.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        IDialogService _dialogService;
        ILoaderService _loaderService;

        public MainPageViewModel(IDialogService dialogService, ILoaderService loaderService)
        {
            _dialogService = dialogService;
            _loaderService = loaderService;
            
        }

        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            return null;
        }
    }
}
