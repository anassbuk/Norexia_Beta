namespace Norexia.Core.Application.Quotations.Commands.CreateQuotation;

public class QuotationLineCommand
{
    public Guid? Id { get; set; }
    public Guid? SellingPriceId { get; set; }
    public int? Qty { get; set; }
}

