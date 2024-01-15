using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Quotations.Queries.GetQuotations;
public record GetQuotationsQuery : IRequest<IEnumerable<QuotationDto>>;

public class GetQuotationsQueryHandler : IRequestHandler<GetQuotationsQuery, IEnumerable<QuotationDto>>
{
    private readonly IApplicationDbContext _context;
    public GetQuotationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<QuotationDto>> Handle(GetQuotationsQuery request, CancellationToken cancellationToken)
    {
        List<QuotationDto> QuotationsDto = new();

        var Quotations = await _context.Quotations
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false)
                                .Include(t => t.QuotationLines)
                                .Include(t => t.PaymentTerms)
                                .ToListAsync(cancellationToken);

        foreach (var quotation in Quotations)
        {
            QuotationDto quotationDto = new()
            {
                Id = quotation.Id,
                Version = quotation.Version,
                Responsable = quotation.Responsable,
                ValidityDuretion = quotation.ValidityDuration,
                CustomerId = quotation.CustomerId,
                Reference = quotation.Reference,
                Discount = quotation.Discount ?? 0,
                QuotationDate = quotation.QuotationDate,
                Note = quotation.Note,
                Status = quotation.Status,

            };

            quotationDto.PriceExcludingTax = quotation.QuotationLines!.Select(line => line.Qty * (line.Price - line.Price * (((double?)line.Discount ?? 0) / 100))).Sum();
            quotationDto.TaxPrice = quotation.QuotationLines!.Select(l => l.Qty * l.Price * ((double?)l.VAT / 100)).Sum();
            quotationDto.PriceIncludingTax = quotationDto.PriceExcludingTax + quotationDto.TaxPrice;
            quotationDto.DiscountPrice = quotationDto.PriceExcludingTax * ((double)quotationDto.Discount / 100);
            quotationDto.NetPrice = quotationDto.PriceIncludingTax - quotationDto.DiscountPrice;
            QuotationsDto.Add(quotationDto);
        }
        return QuotationsDto;


    }
}
