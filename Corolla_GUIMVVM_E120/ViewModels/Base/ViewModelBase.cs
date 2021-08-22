using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Corolla_GUIMVVM_E120.ViewModels.Base
{
    /// <summary>
    /// Aplicación de <see cref="INotifyPropertyChanged"/> para simplificar los modelos.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Frame _appFrame;
        public Frame AppFrame
        {
            get => _appFrame;
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public abstract Task OnNavigatedFrom(NavigationEventArgs args);
        public abstract Task OnNavigatedTo(NavigationEventArgs args);

        /// <summary>   
        /// Evento de multidifusión para las notificaciones de cambios en las propiedades.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Comprueba si una propiedad ya coincide con un valor deseado.  
        /// Establece la propiedad y notifica a los oyentes sólo cuando es necesario.
        /// </summary>
        /// <typeparam name="T">Tipo de propiedad.</typeparam>
        /// <param name="storage">Referencia a una propiedad con getter and setter.</param>
        /// <param name="value">Valor deseado para la propiedad.</param>
        /// <param name="propertyName">Nombre de la propiedad utilizada para notificar a los oyentes.  
        /// Este valor es opcional y puede proporcionarse automáticamente cuando se invoca desde
        /// compiladores que soportan CallerMemberName.</param>
        /// <returns>Verdadero si el valor fue cambiado, falso si el valor existente coincide con el valor deseado.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value))
            {
                return false;
            }
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifica a los oyentes que el valor de una propiedad ha cambiado.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad utilizada para notificar a los oyentes.  
        /// Este valor es opcional y puede proporcionarse automáticamente cuando se invoca 
        /// desde compiladores que soportan <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void SetAppFrame(Frame viewFrame)
        {
            _appFrame = viewFrame;
        }
    }
}
