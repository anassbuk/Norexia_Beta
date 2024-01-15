using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Pages.Settings;
public partial class GeneralSettings
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<VATDto>? VATs { get; set; }
    public List<CurrencyDto>? Currencies { get; set; }
    ToastComponent? Toast;

    protected async override Task OnInitializedAsync()
    {
        VATs = (List<VATDto>)await GCApiProxy!.Proxy.VAT_GetVATsAsync();
        Currencies = (List<CurrencyDto>)await GCApiProxy!.Proxy.Currency_GetCurrenciesAsync();
    }
    private async Task ShowSuccessToast(string content)
    {
        await Toast!.ShowSuccessToast(content);
    }
    private async Task ShowErrorToast(string content)
    {
        await Toast!.ShowErrorToast(content);
    }
}