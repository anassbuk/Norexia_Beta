using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Models.Payment;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.AppState;

namespace NorexiaGestionCommercialeWebUI.Pages.Payment;
public partial class EditPaymentForm
{
    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    NavigationManager? Navigation { get; set; }

    [Inject]
    public States? AppStates { get; set; }

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

        if (AppStates!.Payment is null)
            Navigation!.NavigateTo("/Payments");

        PaymentCommand = Mapper!.Map<PaymentCommand>(AppStates!.Payment);

        PaymentMeans = (List<PaymentMeanDto>)await GCApiProxy!.Proxy.PaymentMean_GetPaymentMeansAsync();


        if (PaymentCommand.PaymentOrigin == PaymentOrigin.Invoice)
        {
            AssociatedPayments = (List<PaymentDto>?)await GCApiProxy!.Proxy.Payment_GetInvoicePaymentsAsync((Guid)PaymentCommand!.InvoiceId!);
            InvoiceInfo = await GCApiProxy.Proxy.Invoice_GetInvoiceAsPaymentAsync(PaymentCommand!.InvoiceRef);
        }
        else
        {
            AssociatedPayments = (List<PaymentDto>?)await GCApiProxy!.Proxy.Payment_GetSaleOrderPaymentsAsync((Guid)PaymentCommand!.SaleOrderId!);
            SaleInfo = await GCApiProxy.Proxy.Payment_GetSaleOrderAsPaymentAsync(PaymentCommand!.SaleOrderRef);
        }

        EC = new EditContext(PaymentCommand);
    }

    public async Task InvoiceInfoChanged(InvoiceAsPaymentDto? invoiceInfo)
    {
        InvoiceInfo = invoiceInfo;
        try
        {
            AssociatedPayments = (List<PaymentDto>?)await GCApiProxy!.Proxy.Payment_GetInvoicePaymentsAsync((Guid)invoiceInfo!.Id!);
        }
        catch (Exception ex)
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
            AssociatedPayments = (List<PaymentDto>?)await GCApiProxy!.Proxy.Payment_GetSaleOrderPaymentsAsync((Guid)saleInfo!.Id!);
        }
        catch (Exception ex)
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
                var updateCommand = Mapper!.Map<UpdatePaymentCommand>(PaymentCommand);
                await GCApiProxy!.Proxy.Payment_UpdatePaymentAsync(Id, updateCommand);
                await Toast!.ShowSuccessToast("Payment edited Successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
