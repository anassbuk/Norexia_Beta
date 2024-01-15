using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.Models.Payment;

namespace NorexiaGestionCommercialeWebUI.Pages.Payment;
public partial class PaymentForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public PaymentCommand PaymentCommand { get; set; } = new();
    public EditContext? EC { get; set; }
    public List<PaymentMeanDto>? PaymentMeans { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    ToastComponent? Toast;
    public InvoiceAsPaymentDto? InvoiceInfo;
    public SaleOrderAsPaymentDto? SaleInfo;
    public List<PaymentDto>? AssociatedPayments;
    protected async override Task OnInitializedAsync()
    {
        EC = new EditContext(PaymentCommand);
        PaymentMeans = (List<PaymentMeanDto>)await GCApiProxy!.Proxy.PaymentMean_GetPaymentMeansAsync();
        AssociatedPayments = new();
        InvoiceInfo = new();
        SaleInfo = new();
    }

    public async Task InvoiceInfoChanged(InvoiceAsPaymentDto? invoiceInfo)
    {
        InvoiceInfo = invoiceInfo;
        try
        {
            if (invoiceInfo!.Id != null)
                AssociatedPayments = (List<PaymentDto>?)await GCApiProxy!.Proxy.Payment_GetInvoicePaymentsAsync((Guid)invoiceInfo!.Id);
            else
                AssociatedPayments = new();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An exception occurred: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }

    public async Task SaleOrderInfoChanged(SaleOrderAsPaymentDto? saleInfo)
    {
        SaleInfo = saleInfo;
        try
        {
            if (saleInfo!.Id != null)
                AssociatedPayments = (List<PaymentDto>?)await GCApiProxy!.Proxy.Payment_GetSaleOrderPaymentsAsync((Guid)saleInfo!.Id);
            else
                AssociatedPayments = new();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An exception occurred: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreatePaymentCommand>(PaymentCommand);
                await GCApiProxy!.Proxy.Payment_CreatePaymentAsync(createCommand);
                await Toast!.ShowSuccessToast("Payment added Successfully");
                Navigation!.NavigateTo("/Payments");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
