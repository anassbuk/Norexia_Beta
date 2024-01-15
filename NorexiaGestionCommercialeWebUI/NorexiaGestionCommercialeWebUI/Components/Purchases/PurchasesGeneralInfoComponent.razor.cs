using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Purchase;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Components.Purchases;
public partial class PurchasesGeneralInfoComponent
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public PurchaseCommand? PurchaseCommand { get; set; }
    [Parameter]
    public ProviderDetailsDto? Provider { get; set; }

    [Parameter]
    public EventCallback<PurchaseCommand> PurchaseCommandChanged { get; set; }

    private string? providerSearchTerm;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;

    protected override void OnParametersSet()
    {
        DisplayProvider();
    }

    public async Task SearchProvider(MouseEventArgs args)
    {
        await GetProvider();
    }

    private async Task GetProvider()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(providerSearchTerm))
            {
                Provider = await GCApiProxy!.Proxy.Provider_GetProviderByReferenceOrNameAsync(providerSearchTerm);

                DisplayProvider();
            }

        }
        catch (Exception)
        {
            DialogMessage = $"Fournisseur avec le terme de recherche '{providerSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    private void DisplayProvider()
    {
        if (Provider != null)
        {
            PurchaseCommand!.ProviderId = Provider.Id;
            providerSearchTerm = $"{Provider.Reference} - {(Provider.ProviderType == ProviderType.Company ? Provider.SocialReason : $"{Provider.FirstName}, {Provider.LastName}")}";
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }
}