using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Corolla_GUIMVVM_E120.Services.LoaderService
{
    public class LoaderService : ILoaderService
    {
        private ResourceLoader loader;

        public LoaderService()
        {
            loader = ResourceLoader.GetForCurrentView();
        }
        public string GetString(string key)
        {
            return loader.GetString(key);
        }
    }
}
