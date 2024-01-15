using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Delivery;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Delivery;
public partial class DeliveryGeneralInfo
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public DeliveryCommand? DeliveryCommand { get; set; }
    [Parameter]
    public EventCallback<DeliveryCommand> DeliveryCommandChanged { get; set; }
    [Parameter]
    public CustomerDetailsDto? Customer { get; set; }
    [Parameter]
    public EventCallback<CustomerDetailsDto> CustomerChanged { get; set; }

    [Parameter]
    public System.Action? OnDisplayCustomerAddress { get; set; }

    private void DisplayCustomerAddress()
    {
        OnDisplayCustomerAddress?.Invoke();
    }

    private string? customerSearchTerm;
    private string? saleSearchTerm;
    private string? invoiceSearchTerm;
    private bool IsDialogVisible;
    private bool IsSaleLinesDialogVisible;
    private bool isPassing = true;
    private string DialogMessage = string.Empty;

    private List<DeliveryLineDto> SaleLines = new();
    private SfGrid<DeliveryLineDto>? SaleLinesGrid;

    protected override void OnParametersSet()
    {
        customerSearchTerm = DeliveryCommand!.CustomerRef;
        saleSearchTerm = DeliveryCommand!.SaleOrderRef;
        invoiceSearchTerm = DeliveryCommand!.InvoiceRef;
    }

    public async Task SearchCustomer(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(customerSearchTerm))
            {
                Customer = await GCApiProxy!.Proxy.Customer_GetCustomerByReferenceOrNameAsync(customerSearchTerm);
                await CustomerChanged.InvokeAsync(Customer);
                DisplayCustomer();
            }
        }
        catch (Exception)
        {
            DialogMessage = $"Client avec le terme de recherche '{customerSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    public async Task SearchSaleOrder(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(saleSearchTerm))
            {
                var sale = await GCApiProxy!.Proxy.SaleOrder_GetSaleOrderAsDeliveryAsync(saleSearchTerm);

                DeliveryCommand!.SaleOrderId = sale.Id;
                DeliveryCommand!.SaleOrderRef = sale.Reference;
                DeliveryCommand.CustomerId = sale.CustomerId;
                customerSearchTerm = sale.CustomerRef;
                DeliveryCommand!.CustomerRef = sale.CustomerRef;

                SaleLines = sale.DeliveryLines.ToList();
                IsSaleLinesDialogVisible = true;
            }

        }
        catch (Exception)
        {
            DialogMessage = $"Commande d'achat avec le terme de recherche '{saleSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }
    public async Task SearchInvoice(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(invoiceSearchTerm))
            {
                var invoice = await GCApiProxy!.Proxy.Invoice_GetInvoiceAsDeliveryAsync(invoiceSearchTerm);

                DeliveryCommand!.InvoiceId = invoice.Id;
                DeliveryCommand!.InvoiceRef = invoice.Reference;
                DeliveryCommand.CustomerId = invoice.CustomerId;
                customerSearchTerm = invoice.CustomerRef;
                DeliveryCommand!.CustomerRef = invoice.CustomerRef;

                SaleLines = invoice.DeliveryLines.ToList();
                IsSaleLinesDialogVisible = true;
            }

        }
        catch (Exception)
        {
            DialogMessage = $"Facture avec le terme de recherche '{saleSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    private void DisplayCustomer()
    {
        if (Customer != null)
        {
            DeliveryCommand!.CustomerId = Customer.Id;
            customerSearchTerm = $"{Customer.Reference} - {(Customer.ClientType == ClientType.Company ? Customer.SocialReason : $"{Customer.FirstName}, {Customer.LastName}")}";
            DisplayCustomerAddress();
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    private async Task SaleLinesDialogOkClick()
    {
        DeliveryCommand!.DeliveryLines = await SaleLinesGrid!.GetSelectedRecordsAsync();
        IsSaleLinesDialogVisible = false;
        await DeliveryCommandChanged.InvokeAsync(DeliveryCommand);
    }

    readonly List<DropDownDeliveryOrigin> ddDeliveryOrigins = new()
    {
        new DropDownDeliveryOrigin() { DisplayName = "Commande de vente", DeliveryOrigin = DeliveryOrigin.SaleOrder},
        new DropDownDeliveryOrigin() { DisplayName = "Création directe", DeliveryOrigin = DeliveryOrigin.DirectCreation},
        new DropDownDeliveryOrigin() { DisplayName = "Facture", DeliveryOrigin = DeliveryOrigin.Facture},
    };

    public class DropDownDeliveryOrigin
    {
        public string? DisplayName { get; set; }
        public DeliveryOrigin DeliveryOrigin { get; set; }
    }

    readonly List<DropDownStockRecordType> ddStockRecordType = new()
    {
        new DropDownStockRecordType() { DisplayName = "Partiel", Type = StockRecordType.Partial},
        new DropDownStockRecordType() { DisplayName = "Complet", Type = StockRecordType.Complete},
    };

    public class DropDownStockRecordType
    {
        public string? DisplayName { get; set; }
        public StockRecordType Type { get; set; }
    }
}
