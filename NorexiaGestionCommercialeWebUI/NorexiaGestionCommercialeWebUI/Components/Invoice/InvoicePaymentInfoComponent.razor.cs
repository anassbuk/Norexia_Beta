using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Invoice;
public partial class InvoicePaymentInfoComponent
{
    [Parameter]
    public OwnedPaymentTerms? DefaultPaymentTerms { get; set; } = new();
    [Parameter]
    public InvoiceCommand? InvoiceCommand { get; set; }

    [Parameter]
    public EventCallback<InvoiceCommand> InvoiceCommandChanged { get; set; }
    [Parameter]
    public System.Action? OnFillDepositInvoiceData { get; set; }

    public SfGrid<PaymentDto>? PaymentsGrid { get; set; }
    [Parameter]
    public List<PaymentMeanDto>? PaymentMeans { get; set; }

    private double SettledAmount = 0;
    private double RemainingAmount = 0;

    private void FillDepositInvoiceData()
    {
        OnFillDepositInvoiceData?.Invoke();
    }

    double? DownPayment;

    protected override void OnParametersSet()
    {
        if (InvoiceCommand!.PaymentTerms == null)
            InvoiceCommand!.PaymentTerms = DefaultPaymentTerms;

        if (InvoiceCommand!.PaymentTerms != null)
        {
            if (InvoiceCommand!.PaymentTerms!.DepositInvoiceDownPayment != null)
                DownPayment = (double)InvoiceCommand!.PaymentTerms!.DepositInvoiceDownPayment / 100;

            if (InvoiceCommand!.EntryDate != null && InvoiceCommand!.PaymentTerms!.MaturityDuration != null)
            {
                InvoiceCommand!.DueDate = InvoiceCommand!.EntryDate?.AddDays((double)InvoiceCommand!.PaymentTerms!.MaturityDuration);
            }

            FillDepositInvoiceData();
        }

        if(InvoiceCommand!.InvoicePayments != null)
            CalcPrices();
    }

    private void DownPaymentValueChanged(ChangeEventArgs<double?> args)
    {
        InvoiceCommand!.PaymentTerms!.DepositInvoiceDownPayment = (int)(DownPayment != null ? DownPayment * 100 : 0);
    }

    private void MaturityDurationValueChanged(ChangeEventArgs<int?> args)
    {
        if (InvoiceCommand!.EntryDate != null && InvoiceCommand!.PaymentTerms!.MaturityDuration != null)
        {
            InvoiceCommand!.DueDate = InvoiceCommand!.EntryDate?.AddDays((double)InvoiceCommand!.PaymentTerms!.MaturityDuration);
        }
    }

    private async Task DepositInvoiceChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
    {
        if (args.Checked == true)
            FillDepositInvoiceData();

        await InvoiceCommandChanged.InvokeAsync(InvoiceCommand);
        StateHasChanged();
    }


    public void OnActionComplete(ActionEventArgs<PaymentDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            CalcPrices();
        }
    }

    private void CalcPrices()
    {
        var priceExcludingTax = (double)InvoiceCommand!.InvoiceLines!.Select(s => s.TotalPriceExcludingTax).Sum()!;
        var priceIncludingTax = (double)InvoiceCommand!.InvoiceLines!.Select(s => s.TotalPriceIncludingTax).Sum()!;
        var discountPrice = priceExcludingTax * (((double?)InvoiceCommand!.Discount ?? 0) / 100);
        var netPrice = priceIncludingTax - discountPrice;

        SettledAmount = InvoiceCommand!.InvoicePayments!.Select(p => p.AmountPaid ?? 0).Sum();
        RemainingAmount = netPrice - SettledAmount;
    }

    private void InvoicePaymentMeanChanged(ChangeEventArgs<Guid?, PaymentMeanDto> args, object context)
    {
        var payment = (context as PaymentDto);
        if (args.Value != null)
        {
            payment!.PaymentMeanId = args.Value;
            payment!.PaymentMeanName = args.ItemData.Name;
        }
    }
}
