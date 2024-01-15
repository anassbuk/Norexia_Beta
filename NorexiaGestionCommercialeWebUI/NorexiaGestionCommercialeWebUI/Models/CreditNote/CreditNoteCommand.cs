using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.CreditNote
{
    public class CreditNoteCommand
    {
        public Guid Id { get; set; }
        public string? CreditNumber { get; set; }  

        public DateTimeOffset? CreditNoteDate { get; set; } = DateTimeOffset.Now;

        public Guid? CustomerId { get; set; }
        public string? CustomerRef { get; set; }
        public string? InvoiceRef { get; set; } 

        public Guid? InvoiceId { get; set; }
        public CreditOrigin? CreditOrigin { get; set; }
        public CreditAction? CreditAction { get; set; }

        public string? Responsable { get; set; }
        public string? Raison { get; set; }
        public string? Note { get; set; }
        public float? Discount { get; set; }
        public double MontantAvoir { get; set; }
        public OwnedPaymentTerms? PaymentTerms { get; set; }
        public DateTimeOffset? DueDate { get; set; } = DateTimeOffset.Now;

        public ICollection<CreditNoteLineDto>? CreditNoteLines { get; set; }


    }
}
