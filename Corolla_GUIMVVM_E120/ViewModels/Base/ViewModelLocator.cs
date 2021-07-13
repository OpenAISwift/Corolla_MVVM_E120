using Corolla_GUIMVVM_E120.Services.DialogService;
using Corolla_GUIMVVM_E120.Services.LoaderService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace Corolla_GUIMVVM_E120.ViewModels.Base
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer _container;

        public ViewModelLocator()
        {
            _container = new UnityContainer();

            _container.RegisterType<MainPageViewModel>();
            _container.RegisterType<SettingsViewModel>();
            _container.RegisterType<ClimateControlViewModel>();

            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILoaderService, LoaderService>(new ContainerControlledLifetimeManager());
        }

        public MainPageViewModel MainPageViewModel
        {
            get { return _container.Resolve<MainPageViewModel>(); }
        }

        public SettingsViewModel SettingsViewModel
        {
            get { return _container.Resolve<SettingsViewModel>(); }
        }

        public ClimateControlViewModel ClimateControlViewModel
        {
            get { return _container.Resolve<ClimateControlViewModel>(); }
        }
    }
}
