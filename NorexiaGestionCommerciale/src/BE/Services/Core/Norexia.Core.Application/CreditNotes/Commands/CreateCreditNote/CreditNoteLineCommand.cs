using MediatR;

namespace Norexia.Core.Application.CreditNotes.Commands.CreateCreditNote
{
    public class CreditNoteLineCommand 
    {
        public Guid? Id { get; set; }
        public Guid? SellingPriceId { get; set; }
        public string? DeliveryRef { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid ProductId { get; set; }
        public double? Price { get; set; }
        public int? VAT { get; set; }
        public int? Discount { get; set; }
        public int? Qty { get; set; }
        public int? ExpectedQty { get; set; }
    }
}