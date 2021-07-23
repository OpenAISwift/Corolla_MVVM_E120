using Corolla_GUIMVVM_E120.ViewModels.Base;
using Corolla_GUIMVVM_E120.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Corolla_GUIMVVM_E120.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        private DispatcherTimer dispatcherTimer;
        private readonly ClockModel _clock;

        public string Hour
        {
            get { return _clock.Hour; }
            set
            {
                _clock.Hour = value;
                RaisePropertyChanged();
            }
        }

        public string Minute
        {
            get { return _clock.Minute; }
            set
            {
                _clock.Minute = value;
                RaisePropertyChanged();
            }
        }

        public string Date
        {
            get { return _clock.Date; }
            set
            {
                _clock.Date = value;
                RaisePropertyChanged();
            }
        }

        public ClockViewModel()
        {
            _clock = new ClockModel();
            InitClock();
        }

        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            return null;
        }

        private void InitClock()
        {
            if (Hour == null)
            {
                Hour = DateTime.Now.ToString(@"HH");
            }
            if (Minute == null)
            {
                Minute = DateTime.Now.ToString(@"mm");
            }
            if (Date == null)
            {
                Date = DateTime.Now.ToString(@"dd/MM/yyyy");
            }
            if (dispatcherTimer == null)
            {
                dispatcherTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(60 - DateTime.Now.Second)
                };
                dispatcherTimer.Tick += DispatcherTimer_Tick;
                dispatcherTimer.Start();
            }
        }

        private void DispatcherTimer_Tick(object sender, object e)
        {
            Hour = DateTime.Now.ToString(@"HH");
            Minute = DateTime.Now.ToString(@"mm");
        }

    }
}
