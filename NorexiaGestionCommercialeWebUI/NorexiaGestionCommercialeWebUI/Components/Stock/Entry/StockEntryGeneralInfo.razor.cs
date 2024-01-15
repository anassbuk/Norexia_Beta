using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components.Product;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Stock.Entry;
public partial class StockEntryGeneralInfo
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public StockEntryCommand? StockEntryCommand { get; set; }
    public ProviderDetailsDto? Provider { get; set; }

    [Parameter]
    public EventCallback<StockEntryCommand> StockEntryCommandChanged { get; set; }

    private string? providerSearchTerm;
    private string? purchaseSearchTerm;
    private bool IsDialogVisible;
    private bool IsPurchaseLinesDialogVisible;
    private string DialogMessage = string.Empty;

    private List<StockEntryLineDto> PurchaseLines = new();
    private SfGrid<StockEntryLineDto>? PurchaseLinesGrid;

    protected override void OnParametersSet()
    {
        providerSearchTerm = StockEntryCommand!.ProviderRef;
        purchaseSearchTerm = StockEntryCommand!.PurchaseOrderRef;
        purchaseSearchTerm = StockEntryCommand!.PurchaseOrderRef;
    }

    public async Task SearchProvider(MouseEventArgs args)
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

    public async Task SearchPurchaseOrder(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(purchaseSearchTerm))
            {
                var purchase = await GCApiProxy!.Proxy.PurchaseOrder_GetPurchaseOrderAsStockEntryAsync(purchaseSearchTerm);

                StockEntryCommand!.PurchaseOrderId = purchase.Id;
                StockEntryCommand!.PurchaseOrderRef = purchase.Reference;
                StockEntryCommand.ProviderId = purchase.ProviderId;
                providerSearchTerm = purchase.ProviderRef;
                StockEntryCommand!.ProviderRef = purchase.ProviderRef;

                PurchaseLines = purchase.StockEntryLines.ToList();
                IsPurchaseLinesDialogVisible = true;
            }

        }
        catch (Exception)
        {
            DialogMessage = $"Commande d'achat avec le terme de recherche '{purchaseSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    private void DisplayProvider()
    {
        if (Provider != null)
        {
            StockEntryCommand!.ProviderId = Provider.Id;
            providerSearchTerm = $"{Provider.Reference} - {(Provider.ProviderType == ProviderType.Company ? Provider.SocialReason : $"{Provider.FirstName}, {Provider.LastName}")}";
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    private async Task PurchaseLinesDialogOkClick()
    {
        StockEntryCommand!.StockEntryLines = await PurchaseLinesGrid!.GetSelectedRecordsAsync();
        IsPurchaseLinesDialogVisible = false;
        await StockEntryCommandChanged.InvokeAsync(StockEntryCommand);
    }

    readonly List<DropDownStockEntryOrigin> ddStockEntryOrigins = new()
    {
        new DropDownStockEntryOrigin() { DisplayName = "Commande d'achat", StockEntryOrigin = StockEntryOrigin.PurchaseOrder},
        new DropDownStockEntryOrigin() { DisplayName = "Création directe", StockEntryOrigin = StockEntryOrigin.DirectCreation},
        new DropDownStockEntryOrigin() { DisplayName = "Retour client", StockEntryOrigin = StockEntryOrigin.CustomerReturn},
        new DropDownStockEntryOrigin() { DisplayName = "Transfert interne", StockEntryOrigin = StockEntryOrigin.InternalTransfer},
    };

    public class DropDownStockEntryOrigin
    {
        public string? DisplayName { get; set; }
        public StockEntryOrigin StockEntryOrigin { get; set; }
    }
}
