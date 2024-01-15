using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Diagrams;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Invoice;
public partial class InvoiceGeneralInfo
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public IMapper? Mapper { get; set; }
    [Parameter]
    public InvoiceCommand? InvoiceCommand { get; set; }
    [Parameter]
    public EventCallback<InvoiceCommand> InvoiceCommandChanged { get; set; }
    [Parameter]
    public CustomerDetailsDto? Customer { get; set; }
    [Parameter]
    public EventCallback<CustomerDetailsDto> CustomerChanged { get; set; }
    [Parameter]
    public EditContext? EC { get; set; }

    private string? customerSearchTerm;
    private string? saleSearchTerm;
    private string? deliverySearchTerm;
    private bool IsDialogVisible;
    private bool IsSaleLinesDialogVisible;
    private string DialogMessage = string.Empty;

    private List<InvoiceLineDto> SaleLines = new();
    private SfGrid<InvoiceLineDto>? SaleLinesGrid;

    protected override void OnParametersSet()
    {
        customerSearchTerm = InvoiceCommand!.CustomerRef;
        saleSearchTerm = InvoiceCommand!.SaleOrderRef;
        deliverySearchTerm = InvoiceCommand!.DeliveryRef;
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
        if (InvoiceCommand!.InvoiceOrigin == InvoiceOrigin.SalesOrder)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(saleSearchTerm))
                {
                    var sale = await GCApiProxy!.Proxy.SaleOrder_GetSaleOrderAsInvoiceAsync(saleSearchTerm);

                    InvoiceCommand!.SaleOrderId = sale.Id;
                    InvoiceCommand!.SaleOrderRef = sale.Reference;
                    InvoiceCommand.CustomerId = sale.CustomerId;
                    customerSearchTerm = sale.CustomerRef;
                    InvoiceCommand!.CustomerRef = sale.CustomerRef;

                    if(sale.PaymentTerms != null)
                        InvoiceCommand!.PaymentTerms = sale.PaymentTerms;

                    SaleLines = sale.InvoiceLines.ToList();

                    InvoiceCommand.InvoicePayments = Mapper!.Map<List<PaymentDto>>(sale.InvoicePayments.ToList());
                    IsSaleLinesDialogVisible = true;
                }

            }
            catch (Exception)
            {
                DialogMessage = $"Commande d'achat avec le terme de recherche '{saleSearchTerm}' introuvable";
                IsDialogVisible = true;
            }
        }
        else if (InvoiceCommand!.InvoiceOrigin == InvoiceOrigin.DeliveryMulti)
        {
            await SearchDeliveries();
        }
    }
    public async Task EntryDateValueChange(ChangedEventArgs<DateTimeOffset?> args)
    {
        if (InvoiceCommand!.EntryDate != null && InvoiceCommand!.PaymentTerms!.MaturityDuration != null)
        {
            InvoiceCommand!.DueDate = InvoiceCommand!.EntryDate?.AddDays((double)InvoiceCommand!.PaymentTerms!.MaturityDuration);
            await InvoiceCommandChanged.InvokeAsync(InvoiceCommand!);
        }
    }
    public async Task SearchDelivery(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(deliverySearchTerm))
            {
                var delivery = await GCApiProxy!.Proxy.Delivery_GetDeliveryAsInvoiceAsync(deliverySearchTerm);

                InvoiceCommand!.DeliveryRef = delivery.Reference;
                InvoiceCommand!.SaleOrderId = delivery.SaleOrderId;
                InvoiceCommand!.SaleOrderRef = delivery.SaleOrderRef;
                InvoiceCommand.CustomerId = delivery.CustomerId;
                customerSearchTerm = delivery.CustomerRef;
                InvoiceCommand!.CustomerRef = delivery.CustomerRef;


                if (delivery.PaymentTerms != null)
                    InvoiceCommand!.PaymentTerms = delivery.PaymentTerms;

                SaleLines = delivery.InvoiceLines.ToList();
                IsSaleLinesDialogVisible = true;
            }

        }
        catch (Exception)
        {
            DialogMessage = $"Bon de livraison avec le terme de recherche '{deliverySearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    public async Task SearchDeliveries()
    {
        var deliveryStartDateField = FieldIdentifier.Create(() => InvoiceCommand!.DeliveryStartDate);
        EC!.NotifyFieldChanged(deliveryStartDateField);
        var startDateValidationMessages = EC!.GetValidationMessages(deliveryStartDateField);

        if (!startDateValidationMessages.Any())
        {
            var deliveryEndDateField = FieldIdentifier.Create(() => InvoiceCommand!.DeliveryEndDate);
            EC!.NotifyFieldChanged(deliveryEndDateField);
            var endDateValidationMessages = EC!.GetValidationMessages(deliveryEndDateField);

            if (!string.IsNullOrWhiteSpace(saleSearchTerm) && !endDateValidationMessages.Any())
            {
                try
                {
                    FromToDate fromToDate = new()
                    {
                        StartDate = (DateTimeOffset)InvoiceCommand!.DeliveryStartDate!,
                        EndDate = (DateTimeOffset)InvoiceCommand!.DeliveryEndDate!
                    };
                    var delivery = await GCApiProxy!.Proxy.Delivery_GetDeliveriesAsInvoiceAsync(saleSearchTerm, fromToDate);

                    InvoiceCommand!.SaleOrderId = delivery.SaleOrderId;
                    InvoiceCommand!.SaleOrderRef = delivery.SaleOrderRef;
                    InvoiceCommand.CustomerId = delivery.CustomerId;
                    customerSearchTerm = delivery.CustomerRef;
                    InvoiceCommand!.CustomerRef = delivery.CustomerRef;


                    if (delivery.PaymentTerms != null)
                        InvoiceCommand!.PaymentTerms = delivery.PaymentTerms;

                    SaleLines = delivery.InvoiceLines.ToList();
                    IsSaleLinesDialogVisible = true;
                }
                catch (Exception)
                {
                    DialogMessage = $"Bon de commande avec le terme de recherche '{saleSearchTerm}' introuvable";
                    IsDialogVisible = true;
                }
            }
        }
    }

    private void DisplayCustomer()
    {
        if (Customer != null)
        {
            InvoiceCommand!.CustomerId = Customer.Id;
            customerSearchTerm = $"{Customer.Reference} - {(Customer.ClientType == ClientType.Company ? Customer.SocialReason : $"{Customer.FirstName}, {Customer.LastName}")}";
            //DisplayCustomerAddress();
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    private async Task SaleLinesDialogOkClick()
    {
        InvoiceCommand!.InvoiceLines = await SaleLinesGrid!.GetSelectedRecordsAsync();
        IsSaleLinesDialogVisible = false;
        await InvoiceCommandChanged.InvokeAsync(InvoiceCommand);
    }

    readonly List<DropDownInvoiceOrigin> ddInvoiceOrigins = new()
    {
        new DropDownInvoiceOrigin() { DisplayName = "Commande de vente", InvoiceOrigin = InvoiceOrigin.SalesOrder},
        new DropDownInvoiceOrigin() { DisplayName = "Création directe", InvoiceOrigin = InvoiceOrigin.DirectCreation},
        new DropDownInvoiceOrigin() { DisplayName = "Livraison (Mono)", InvoiceOrigin = InvoiceOrigin.DeliveryMono},
        new DropDownInvoiceOrigin() { DisplayName = "Livraison (Multi)", InvoiceOrigin = InvoiceOrigin.DeliveryMulti},
    };

    public class DropDownInvoiceOrigin
    {
        public string? DisplayName { get; set; }
        public InvoiceOrigin InvoiceOrigin { get; set; }
    }

    readonly List<DropDownInvoiceType> ddInvoiceTypes = new()
    {
        new DropDownInvoiceType() { DisplayName = "Standard", InvoiceType = InvoiceType.Standard},
        new DropDownInvoiceType() { DisplayName = "Pro-forma", InvoiceType = InvoiceType.Proforma},
    };

    public class DropDownInvoiceType
    {
        public string? DisplayName { get; set; }
        public InvoiceType InvoiceType { get; set; }
    }
}
