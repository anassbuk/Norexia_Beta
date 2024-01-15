using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Microsoft.AspNetCore.Components.Web;
using NorexiaGestionCommercialeWebUI.Models.Payment;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.DropDowns;

namespace NorexiaGestionCommercialeWebUI.Components.Payment;
public partial class PaymentGeneralInfo
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public PaymentCommand? PaymentCommand { get; set; }
    [Parameter]
    public EventCallback<PaymentCommand> PaymentCommandChanged { get; set; }
    [Parameter]
    public List<PaymentMeanDto>? PaymentMeans { get; set; }

    [Parameter]
    public bool IsEdit { get; set; }
    public InvoiceAsPaymentDto? InvoiceInfo { get; set; }
    public SaleOrderAsPaymentDto? SaleInfo { get; set; }

    [Parameter]
    public EventCallback<InvoiceAsPaymentDto?> InvoiceAsPaymentChanged { get; set; }

    [Parameter]
    public EventCallback<SaleOrderAsPaymentDto?> SaleOrderAsPaymentChanged { get; set; }

    private string? invoiceSearchTerm;
    private string? saleSearchTerm;
    private double? amountToBePaidPercentage;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;

    private Guid? SelectedPaymentMeanId;

    protected override void OnParametersSet()
    {
        invoiceSearchTerm = PaymentCommand!.InvoiceRef;
        saleSearchTerm = PaymentCommand!.SaleOrderRef;
        SelectedPaymentMeanId = PaymentCommand!.PaymentMeanId;
    }

    public void OnPaymentMeanChanged(SelectEventArgs<PaymentMeanDto> args)
    {
        PaymentCommand!.PaymentMeanId = args.ItemData.Id;
    }

    protected override void OnInitialized()
    {
        if (PaymentCommand!.AmountToBePaidPercentage != null)
            amountToBePaidPercentage = (double?)PaymentCommand!.AmountToBePaidPercentage / 100;
    }
    private async Task PaymentOriginChanged(ChangeEventArgs<PaymentOrigin?, DropDownPaymentOrigin> args)
    {

        PaymentCommand!.InvoiceId = null;
        PaymentCommand!.InvoiceRef = null;
        PaymentCommand!.DueDate = null;

        PaymentCommand!.SaleOrderId = null;
        PaymentCommand!.SaleOrderRef = null;

        await InvoiceAsPaymentChanged.InvokeAsync(new());
    }

    public async Task SearchInvoice(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(invoiceSearchTerm))
            {
                InvoiceInfo = await GCApiProxy!.Proxy.Invoice_GetInvoiceAsPaymentAsync(invoiceSearchTerm);

                PaymentCommand!.InvoiceId = InvoiceInfo.Id;
                PaymentCommand!.InvoiceRef = InvoiceInfo.Reference;
                PaymentCommand!.DueDate = InvoiceInfo.DueDate;

                await InvoiceAsPaymentChanged.InvokeAsync(InvoiceInfo);
            }
        }
        catch (Exception)
        {
            DialogMessage = $"Facture avec le terme de recherche '{invoiceSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    public async Task SearchSaleOrder(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(saleSearchTerm))
            {
                SaleInfo = await GCApiProxy!.Proxy.Payment_GetSaleOrderAsPaymentAsync(saleSearchTerm);

                PaymentCommand!.SaleOrderId = SaleInfo.Id;
                PaymentCommand!.SaleOrderRef = SaleInfo.Reference;

                await SaleOrderAsPaymentChanged.InvokeAsync(SaleInfo);
            }
        }
        catch (Exception)
        {
            DialogMessage = $"Commande de vente avec le terme de recherche '{saleSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    private void AmountToBePaidPercentageChange(ChangeEventArgs<double?> args)
    {
        if (args.Value.HasValue)
        {
            if (PaymentCommand!.PaymentOrigin == PaymentOrigin.Invoice)
            {
                PaymentCommand!.AmountToBePaidPercentage = ((float?)args.Value ?? 0) * 100;
                PaymentCommand!.AmountToBePaid = amountToBePaidPercentage * (InvoiceInfo?.TotalPriceIncludingVAT ?? 0);
            }
            else
            {
                PaymentCommand!.AmountToBePaidPercentage = ((float?)args.Value ?? 0) * 100;
                PaymentCommand!.AmountToBePaid = amountToBePaidPercentage * (SaleInfo?.TotalPriceIncludingVAT ?? 0);
            }
        }
    }

    private void AmountToBePaidChange(ChangeEventArgs<double?> args)
    {
        if (args.Value.HasValue)
        {
            if (PaymentCommand!.PaymentOrigin == PaymentOrigin.Invoice)
            {
                amountToBePaidPercentage = args.Value / (InvoiceInfo?.TotalPriceIncludingVAT ?? 0);
                PaymentCommand!.AmountToBePaidPercentage = ((float?)amountToBePaidPercentage ?? 0) * 100;
            }
            else
            {
                amountToBePaidPercentage = args.Value / (SaleInfo?.TotalPriceIncludingVAT ?? 0);
                PaymentCommand!.AmountToBePaidPercentage = ((float?)amountToBePaidPercentage ?? 0) * 100;
            }
        }
    }

    private void DialogOkClick()
    {
        IsDialogVisible = false;
    }

    readonly List<DropDownPaymentOrigin> ddPaymentOrigins = new()
    {
        new DropDownPaymentOrigin() { DisplayName = "Facture", PaymentOrigin = PaymentOrigin.Invoice},
        new DropDownPaymentOrigin() { DisplayName = "Commande de vente", PaymentOrigin = PaymentOrigin.SaleOrder},
    };

    public class DropDownPaymentOrigin
    {
        public string? DisplayName { get; set; }
        public PaymentOrigin PaymentOrigin { get; set; }
    }
}
