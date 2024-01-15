using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;
using Norexia.Core.Domain.QuotationEntities;

namespace Norexia.Core.Application.Quotations.Commands.CreateQuotation;

internal class CreateQuoationCommandHandler : IRequestHandler<CreateQuotationCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateQuoationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
    {

        if (await _context.Quotations.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Quotaion with refrence({request.Reference}) Already exixst !");


        if (request.CustomerId != null)
        {
            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId))
                throw new NotFoundException($"Customer with id({request.CustomerId}) Not Found !");
        }

        Quotation quotation = new()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            QuotationDate = request.QuotationDate!.Value.ToUniversalTime(),
            ValidityDuration = request.ValidityDuretion,
            Responsable = request.Responsable,
            CustomerId = request.CustomerId,
            Status = request.Status,
            Discount = request.Discount,
            Note = request.Note,
            Version = request.Version,
            PaymentTerms = request.PaymentTerms,
            DeliveryDate = request.DeliveryDate!.Value.ToUniversalTime(),
            DeliveryMode = request.DeliveryMode,

        };
        quotation.QuotationLines = await HandleQuotationLines(request, cancellationToken);

        var result = await _context.Quotations.AddAsync(quotation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;


    }

    private async Task<ICollection<QuotationLine>?> HandleQuotationLines(CreateQuotationCommand request, CancellationToken cancellationToken)
    {
        List<QuotationLine> quotationLines = new();
        foreach (var line in request.QuotationLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(line.SellingPriceId, cancellationToken);

            if (sellingPrice is null)
                throw new NotFoundException($"Selling Price with id ({request.CustomerId}) not found! ");

            quotationLines.Add(new QuotationLine()
            {
                ProductId = sellingPrice.ProductId,
                SellingPriceId = line.SellingPriceId,
                Price = sellingPrice.Price,
                VAT = sellingPrice.VAT,
                Margin = sellingPrice.Margin,
                Discount = sellingPrice.Discount,
                Qty = line.Qty,
            });

        }
        return quotationLines;
    }

}
