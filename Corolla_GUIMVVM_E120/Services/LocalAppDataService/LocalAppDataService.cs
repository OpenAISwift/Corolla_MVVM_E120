using Corolla_GUIMVVM_E120.Contants;
using System;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

namespace Corolla_GUIMVVM_E120.Services.LocalAppDataService
{
    public class LocalAppDataService : ILocalAppDataService
    {
        private readonly ResourceLoader loader;
        private readonly ApplicationDataContainer localSettings;

        public LocalAppDataService()
        {
            loader = ResourceLoader.GetForCurrentView();
            localSettings = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// Comprueba si existe el contenedor 
        /// </summary>
        /// <param name="localDataType"></param>
        /// <returns></returns>
        public bool ContainerLocalAppDataExists(LocalDataType localDataType)
        {
            string nameContainer = Enum.GetName(typeof(LocalDataType), localDataType);
            bool hasContainer = localSettings.Containers.ContainsKey(nameContainer);
            if (hasContainer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Crea un nuevo contenedor
        /// </summary>
        /// <param name="localDataType"></param>
        public void CreateContainerLocalAppData(LocalDataType localDataType)
        {
            string nameContainer = Enum.GetName(typeof(LocalDataType), localDataType);
            _ = localSettings.CreateContainer(nameContainer, ApplicationDataCreateDisposition.Always);
        }

        /// <summary>
        /// Actualizar un contenedor compuesto.
        /// </summary>
        /// <param name="localDataType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCompositeLocalAppData(LocalDataType localDataType, ApplicationDataCompositeValue composite)
        {
            string nameContainer = Enum.GetName(typeof(LocalDataType), localDataType);
            localSettings.Values[nameContainer] = composite;
        }

        /// <summary>
        /// Recupera los valores almacenado en un contenedor
        /// </summary>
        /// <param name="localDataType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public ApplicationDataCompositeValue GetContainerLocalAppData(LocalDataType localDataType)
        {
            string nameContainer = Enum.GetName(typeof(LocalDataType), localDataType);

            ApplicationDataCompositeValue keyValuePairs = (ApplicationDataCompositeValue)localSettings.Values[nameContainer];

            if (keyValuePairs ==null)
            {
                return null;
            }
            else
            {
                return keyValuePairs;
            }
        }

        /// <summary>
        /// Crear o actualizar configuraciones simples de un contenedor.
        /// </summary>
        /// <param name="localDataType"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetContainerLocalAppData(LocalDataType localDataType, string key, object value)
        {
            string nameContainer = Enum.GetName(typeof(LocalDataType), localDataType);
            localSettings.Containers[nameContainer].Values[key] = value;
        }

       

        /// <summary>
        /// Elimina un contenedor
        /// </summary>
        /// <param name="localDataType"></param>
        public void RemoveLocalContainerAppData(LocalDataType localDataType)
        {
            string nameContainer = Enum.GetName(typeof(LocalDataType), localDataType);

            localSettings.DeleteContainer(nameContainer);
        }

    }
}
