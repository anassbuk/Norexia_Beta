using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Quotation;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Quotations
{
    public partial class QuotationPaymentInfoComponent
    {
        [Parameter]
        public OwnedPaymentTerms? DefaultPaymentTerms { get; set; } = new();
        [Parameter]
        public QuotationCommand? QuotationCommand { get; set; }

        [Parameter]
        public EventCallback<QuotationCommand> QuotationCommandChanged { get; set; }

        double? DownPayment;

        protected override void OnParametersSet()
        {
            if (QuotationCommand!.PaymentTerms == null)
                QuotationCommand!.PaymentTerms = DefaultPaymentTerms;

            if (QuotationCommand!.PaymentTerms != null && QuotationCommand!.PaymentTerms!.DepositInvoiceDownPayment != null)
                DownPayment = (double)QuotationCommand!.PaymentTerms!.DepositInvoiceDownPayment / 100;
        }

        private void DownPaymentValueChanged(ChangeEventArgs<double?> args)
        {
            QuotationCommand!.PaymentTerms!.DepositInvoiceDownPayment = (int)(DownPayment != null ? DownPayment * 100 : 0);
        }   
    }
}
