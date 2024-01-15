using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.CreditNote;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.CreditNote
{
    public partial class CreditNotePaymentInfoComponent
    {
        [Parameter]
        public OwnedPaymentTerms? DefaultPaymentTerms { get; set; } = new();
        [Parameter]
        public CreditNoteCommand? CreditNoteCommand { get; set; }
        [Parameter]
        public EventCallback<CreditNoteCommand> CreditNoteCommandChanged { get; set; }
        [Parameter]
        public System.Action? OnFillDepositCreditNoteData { get; set; }

        private void FillDepositCreditNoteData()
        {
            OnFillDepositCreditNoteData?.Invoke();
        }

        double? DownPayment;
        
        protected override void OnParametersSet()
        {
            if(CreditNoteCommand!.PaymentTerms == null)
                CreditNoteCommand!.PaymentTerms = DefaultPaymentTerms;

            if (CreditNoteCommand!.PaymentTerms != null)
            {
                if(CreditNoteCommand!.PaymentTerms!.DepositInvoiceDownPayment != null)
                    DownPayment = (double)CreditNoteCommand!.PaymentTerms!.DepositInvoiceDownPayment / 100;

                if (CreditNoteCommand!.CreditNoteDate != null && CreditNoteCommand!.PaymentTerms!.MaturityDuration != null)
                {
                    CreditNoteCommand!.DueDate = CreditNoteCommand!.CreditNoteDate?.AddDays((double)CreditNoteCommand!.PaymentTerms!.MaturityDuration);
                }

                FillDepositCreditNoteData();
            }
        }
      
        private void MaturityDurationValueChanged(ChangeEventArgs<int?> args)
        {
            if (CreditNoteCommand!.CreditNoteDate != null && CreditNoteCommand!.PaymentTerms!.MaturityDuration != null)
            {
                CreditNoteCommand!.DueDate = CreditNoteCommand!.CreditNoteDate?.AddDays((double)CreditNoteCommand!.PaymentTerms!.MaturityDuration);
            }
        }

        
        }
}
