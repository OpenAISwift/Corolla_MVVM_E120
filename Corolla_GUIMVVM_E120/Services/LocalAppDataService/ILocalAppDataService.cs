using Corolla_GUIMVVM_E120.Contants;

using Windows.Storage;

namespace Corolla_GUIMVVM_E120.Services.LocalAppDataService
{
    public interface ILocalAppDataService
    {
        bool ContainerLocalAppDataExists(LocalDataType localDataType);
        void CreateContainerLocalAppData(LocalDataType localDataType);
        void SetCompositeLocalAppData(LocalDataType localDataType, ApplicationDataCompositeValue composite);

        ApplicationDataCompositeValue GetContainerLocalAppData(LocalDataType localDataType);

        void SetContainerLocalAppData(LocalDataType localDataType, string key, object value);
        void RemoveLocalContainerAppData(LocalDataType localDataType);
    }
}
