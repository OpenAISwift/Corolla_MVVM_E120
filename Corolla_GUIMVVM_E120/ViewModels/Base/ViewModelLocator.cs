using Corolla_GUIMVVM_E120.Services.DialogService;
using Corolla_GUIMVVM_E120.Services.LoaderService;
using Corolla_GUIMVVM_E120.Services.LocalAppDataService;
using Corolla_GUIMVVM_E120.Services.SerialDeviceService;

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

            _container.RegisterType<ClockViewModel>();
            _container.RegisterType<WarningLightsViewModel>();

            _container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILoaderService, LoaderService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILocalAppDataService, LocalAppDataService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISerialDeviceService, SerialDeviceService>(new ContainerControlledLifetimeManager());
        }

        public MainPageViewModel MainPageViewModel => _container.Resolve<MainPageViewModel>();
        public SettingsViewModel SettingsViewModel => _container.Resolve<SettingsViewModel>();
        public ClimateControlViewModel ClimateControlViewModel => _container.Resolve<ClimateControlViewModel>();

        public ClockViewModel ClockViewModel => _container.Resolve<ClockViewModel>();
        public WarningLightsViewModel WarningLightsViewModel => _container.Resolve<WarningLightsViewModel>();
    }
}
