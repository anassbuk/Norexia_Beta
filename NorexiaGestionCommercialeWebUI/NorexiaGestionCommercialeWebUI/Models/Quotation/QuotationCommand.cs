using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Quotation
{
    public class QuotationCommand
    {
        public Guid Id { get; set; }
        public string? Reference { get; set; }
        public DateTimeOffset QuotationDate { get; set; }
        public int? ValidityDuretion { get; set; }
        public string? Responsable { get; set; }
        public Guid? CustomerId { get; set; }
        public string? Status { get; set; }
        public float? Discount { get; set; }
        public string? Note { get; set; }
        public int? Version { get; set; } = 1;
        public OwnedPaymentTerms? PaymentTerms { get; set; }
        public DateTimeOffset? DeliveryDate { get; set; } = DateTime.Now;
        public DeliveryMode? DeliveryMode { get; set; }
        public  ICollection<QuotationLineDto>? QuotationLines { get; set; }
       
    }
}
