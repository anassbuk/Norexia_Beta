using Syncfusion.Blazor;

namespace NorexiaGestionCommercialeWebUI.Shared
{
    public class SyncfusionLocalizer : ISyncfusionStringLocalizer
    {
        // To get the locale key from mapped resources file
        public string GetText(string key)
        {
            return ResourceManager.GetString(key)!;
        }

        // To access the resource file and get the exact value for locale key
        public System.Resources.ResourceManager ResourceManager
        {
            get
            {
                return NorexiaGestionCommercialeWebUI.Resources.SfResources.ResourceManager;
            }
        }
    }
}
