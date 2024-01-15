using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;

namespace NorexiaGestionCommercialeWebUI.Components.Sales;
public partial class SalesGeneralInfoComponent
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public SfMultiSelect<string[], ProductAvailabilityDto>? SfMultiSelect;

    [Parameter]
    public List<ProductAvailabilityDto> ProductAvailablities { get; set; } = new();
    [Parameter]
    public CustomerDetailsDto? Customer { get; set; }
    [Parameter]
    public SaleCommand? SaleCommand { get; set; }

    [Parameter]
    public EventCallback<SaleCommand> SaleCommandChanged { get; set; }

    private CustomerAddressDto? BillingCustomerAddress;
    private CustomerAddressDto? DeliveryCustomerAddress;
    private string? clientSearchTerm;
    private string? quotationSearchTerm;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;
    private bool isPassing = true;
    private Guid? SelectedSaleChannelId;

    protected override void OnParametersSet()
    {
        DisplayClientAddress();
        SelectedSaleChannelId = SaleCommand!.SaleChannelId;
        quotationSearchTerm = SaleCommand.QuotationRef;
    }

    private async Task OnExecutionChange(ChangeEventArgs<SaleExecution?, DropDownSaleExecution> args)
    {
        await SaleCommandChanged.InvokeAsync(SaleCommand);
    }

    public void OnProductAvailabilityChanged(SelectEventArgs<ProductAvailabilityDto> args)
    {
        SaleCommand!.SaleChannelId = args.ItemData.Id;
    }

    private async Task SearchClient(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(clientSearchTerm))
            {
                Customer = await GCApiProxy!.Proxy.Customer_GetCustomerByReferenceOrNameAsync(clientSearchTerm);

                DisplayClientAddress();
            }

        }
        catch (Exception)
        {
            DialogMessage = $"Client avec le terme de recherche '{clientSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    private async Task SearchQuotation(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(quotationSearchTerm))
            {
                var quotation = await GCApiProxy!.Proxy.SaleOrder_GetQuotationAsSaleOrderAsync(quotationSearchTerm);

                SaleCommand!.QuotationId = quotation.Id;
                SaleCommand!.CustomerId = quotation.CustomerId;
                SaleCommand!.Discount = quotation.Discount;
                SaleCommand!.PaymentTerms = quotation.PaymentTerms;
                SaleCommand!.DeliveryDate = quotation.DeliveryDate;
                SaleCommand!.DeliveryMode = quotation.DeliveryMode;
                SaleCommand!.SaleOrderLines = quotation.SaleOrderLines;
                SaleCommand.QuotationRef = quotation.Reference;

                if (quotation.CustomerId != null)
                {
                    clientSearchTerm = quotation.CustomerRef;
                    BillingCustomerAddress = quotation.BillingCustomerAddress;
                    DeliveryCustomerAddress = quotation.DeliveryCustomerAddress;
                    isPassing = false;
                }
                else
                    isPassing = true;

                await SaleCommandChanged.InvokeAsync(SaleCommand);
            }
        }
        catch (Exception)
        {
            DialogMessage = $"Devis avec le terme de recherche '{quotationSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    private void DisplayClientAddress()
    {
        if (Customer != null)
        {
            SaleCommand!.CustomerId = Customer.Id;
            clientSearchTerm = $"{Customer.Reference} - {(Customer.ClientType == ClientType.Company ? Customer.SocialReason : $"{Customer.FirstName}, {Customer.LastName}")}";

            if (Customer.CustomerAddresses != null)
            {
                BillingCustomerAddress = Customer.CustomerAddresses.FirstOrDefault(a => a.AddressType != AddressType.Delivery && a.Active == true);
                DeliveryCustomerAddress = Customer.CustomerAddresses.FirstOrDefault(a => a.AddressType != AddressType.Billing && a.Active == true);
            }
        }
    }

    private async Task OperationTypeChangeHandler(ChangeEventArgs<SaleOperationType?, DropDownSaleOperationType> args)
    {
        if(args.ItemData.OperationType == SaleOperationType.OrderTaking)
        {
            SaleCommand!.Execution = SaleExecution.Scheduled;
            await SaleCommandChanged.InvokeAsync(SaleCommand);
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    readonly List<DropDownSaleOperationType> ddSaleOperationType = new()
    {
        new DropDownSaleOperationType() { DisplayName = "Vente au comptoir", OperationType = SaleOperationType.CounterSale},
        new DropDownSaleOperationType() { DisplayName = "Prise de commande", OperationType = SaleOperationType.OrderTaking},
    };

    public class DropDownSaleOperationType
    {
        public string? DisplayName { get; set; }
        public SaleOperationType OperationType { get; set; }
    }

    readonly List<DropDownSaleExecution> ddSaleExecution = new()
    {
        new DropDownSaleExecution() { DisplayName = "Immédiate", Execution = SaleExecution.Immediate},
        new DropDownSaleExecution() { DisplayName = "Planifiée", Execution = SaleExecution.Scheduled},
    };

    public class DropDownSaleExecution
    {
        public string? DisplayName { get; set; }
        public SaleExecution Execution { get; set; }
    }

    public class DropDownSaleOrderOrigin
    {
        public string? DisplayName { get; set; }
        public SaleOrderOrigin SaleOrderOrigin { get; set; }
    }

    readonly List<DropDownSaleOrderOrigin> ddSaleOrderOrigin = new()
    {
        new DropDownSaleOrderOrigin() { DisplayName = "Création derict", SaleOrderOrigin = SaleOrderOrigin.DirectCreation},
        new DropDownSaleOrderOrigin() { DisplayName = "Devis", SaleOrderOrigin = SaleOrderOrigin.Quotation},
    };
}
