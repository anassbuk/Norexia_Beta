using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Net;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductSalesComponent;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class PaymentTermsSettings
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public PaymentTermsDto? PaymentTerms { get; set; } = new();
    private EditContext? EC { get; set; }
    double? DownPayment;
    bool? Negotiable;

    [Inject]
    public IMapper? Mapper { get; set; }
    [Parameter]
    public EventCallback<string> OnShowSuccessToast { get; set; }

    protected async Task ShowSuccessToast(string content)
    {
        await OnShowSuccessToast.InvokeAsync(content);
    }

    [Parameter]
    public EventCallback<string> OnShowErrorToast { get; set; }

    protected async Task ShowErrorToast(string content)
    {
        await OnShowErrorToast.InvokeAsync(content);
    }

    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(PaymentTerms!);

        try
        {
            PaymentTerms = await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync();
        }
        catch
        {
            //if (ex.InnerException is WebException webException)
            //{
            //    if (webException.Response is HttpWebResponse httpResponse)
            //    {
            //        if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            //            return;
            //    }
            //}
            //await ShowErrorToast(ex.Message);
        }

        if(PaymentTerms!.DepositInvoiceNegotiable == true)
            Negotiable = true;

        if(PaymentTerms!.DepositInvoiceDownPayment != null)
            DownPayment = (double)PaymentTerms!.DepositInvoiceDownPayment / 100;

        EC = new EditContext(PaymentTerms!);
    }

    public async Task EditPaymentTerms()
    {
        if (EC!.Validate())
        {
            try
            {
                var paymentTermsCommand = Mapper!.Map<UpdatePaymentTermsCommand>(PaymentTerms);
                await GCApiProxy!.Proxy.PaymentTerms_UpdatePaymentTermsAsync(paymentTermsCommand);
                await ShowSuccessToast("Payment terms saved Successfully");
            }
            catch (Exception ex)
            {
                await ShowErrorToast(ex.Message);
            }
        }
    }

    public void OnNegotiableChanged(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
    {
        if (args.Checked == true)
        {
            PaymentTerms!.MaturityDurationNegotiable = true;
            PaymentTerms!.DepositInvoiceNegotiable = true;
            PaymentTerms!.PaymentByInstallmentsNegotiable = true;
        }
        else
        {
            PaymentTerms!.MaturityDurationNegotiable = false;
            PaymentTerms!.DepositInvoiceNegotiable = false;
            PaymentTerms!.PaymentByInstallmentsNegotiable = false;
        }
    }

    private void DownPaymentValueChanged(ChangeEventArgs<double?> args)
    {
        PaymentTerms!.DepositInvoiceDownPayment = (int)(DownPayment != null ? DownPayment * 100 : 0);
    }

    readonly List<DropDownPaymentOption> ddPaymentOptions = new()
    {
        new DropDownPaymentOption() { DisplayName = "Paiement à la livraison", PaymentOption = PaymentOption.CashOnDelivery},
        new DropDownPaymentOption() { DisplayName = "Paiement à la commande (anticipé)", PaymentOption = PaymentOption.PaymentOnOrder},
        new DropDownPaymentOption() { DisplayName = "Paiement sur facture", PaymentOption = PaymentOption.PaymentOnInvoice},
    };

    public class DropDownPaymentOption
    {
        public string? DisplayName { get; set; }
        public PaymentOption PaymentOption { get; set; }
    }
}
