using Microsoft.Extensions.Options;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Infrastructure;

namespace NorexiaGestionCommercialeWebUI.Proxies;
public class GestionCommercialApiProxy
{
    public GestionCommercialApiProxy(HttpClient httpClient, IOptions<GestionCommercialApiSettings> gestionCommercialApiSettings)
    {
        Proxy = new NorexiaCoreFacadeClient(gestionCommercialApiSettings.Value.RootServiceUrl, httpClient);
    }

    public NorexiaCoreFacadeClient Proxy { get; private set; }
}
