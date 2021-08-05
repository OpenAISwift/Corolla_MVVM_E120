using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Corolla_GUIMVVM_E120.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region Declaracion de eventos

        /// <summary>   
        /// Evento de multidifusión para las notificaciones de cambios en las propiedades.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

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
        /// <returns>Verdadero si el valor fue cambiado, 
        /// falso si el valor existente coincide con el valor deseado.</returns>
        protected bool RaisePropertyChanged<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
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
    }
}
