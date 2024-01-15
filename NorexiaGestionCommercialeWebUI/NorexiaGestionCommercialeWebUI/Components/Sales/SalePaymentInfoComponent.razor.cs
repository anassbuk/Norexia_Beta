using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Sales
{
    public partial class SalePaymentInfoComponent
    {
        [Parameter]
        public OwnedPaymentTerms? DefaultPaymentTerms { get; set; } = new();
        [Parameter]
        public SaleCommand? SaleCommand { get; set; }

        [Parameter]
        public EventCallback<SaleCommand> SaleCommandChanged { get; set; }

        double? DownPayment;
        public SfGrid<PaymentDto>? PaymentsGrid { get; set; }

        [Parameter]
        public List<PaymentMeanDto>? PaymentMeans { get; set; }

        protected override void OnParametersSet()
        {
            if (SaleCommand!.PaymentTerms != null && SaleCommand!.PaymentTerms!.DepositInvoiceDownPayment != null)
                DownPayment = (double)SaleCommand!.PaymentTerms!.DepositInvoiceDownPayment / 100;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (SaleCommand!.PaymentTerms == null)
                SaleCommand!.PaymentTerms = DefaultPaymentTerms;
        }

        private void DownPaymentValueChanged(ChangeEventArgs<double?> args)
        {
            SaleCommand!.PaymentTerms!.DepositInvoiceDownPayment = (int)(DownPayment != null ? DownPayment * 100 : 0);
        }

        public void OnActionComplete(ActionEventArgs<PaymentDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                //CalcTotalPrices();
            }
        }

        private void SalePaymentMeanChanged(ChangeEventArgs<Guid?, PaymentMeanDto> args, object context)
        {
            var payment = (context as PaymentDto);
            if (args.Value != null)
            {
                payment!.PaymentMeanId = args.Value;
                payment!.PaymentMeanName = args.ItemData.Name;
            }
        }
    }
}
