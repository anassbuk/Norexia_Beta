using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;
using Norexia.Core.Domain.QuotationEntities;

namespace Norexia.Core.Application.Quotations.Commands.UpdateQuotation;

internal class UpdateQuotationCommandHandler : IRequestHandler<UpdateQuotationCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateQuotationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(UpdateQuotationCommand request, CancellationToken cancellationToken)
    {
        var quotation = await _context.Quotations
                        .Include(q => q.QuotationLines)
                         .SingleOrDefaultAsync(q => q.Id == request.Id, cancellationToken);

        if (quotation == null)
            throw new NotFoundException($"Quotation with id ({request.Id}) not found! ");

        if (request.CustomerId != null)
        {
            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken))
                throw new NotFoundException($"Customer with id ({request.CustomerId}) not found! ");
        }
        quotation.Reference = request.Reference;
        quotation.QuotationDate = request.QuotationDate!.Value.ToUniversalTime();
        quotation.ValidityDuration = request.ValidityDuretion;
        quotation.Responsable = request.Responsable;
        quotation.CustomerId = request.CustomerId;
        quotation.Status = request.Status;
        quotation.Note = request.Note;
        quotation.Version = request.Version;
        quotation.PaymentTerms = request.PaymentTerms;
        quotation.DeliveryDate = request.DeliveryDate?.ToUniversalTime();
        quotation.DeliveryMode = request.DeliveryMode;

        await HandleQuotationLines(request, quotation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return quotation.Id;

    }

    private async Task HandleQuotationLines(UpdateQuotationCommand request, Quotation quotation, CancellationToken cancellationToken)
    {
        var linesToRemove = quotation.QuotationLines!
            .Where(l => !request.QuotationLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            quotation.QuotationLines!.Remove(line);

        foreach (var item in request.QuotationLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(item.SellingPriceId, cancellationToken);

            if (sellingPrice == null)
                throw new NotFoundException($"Selling Price with id ({request.CustomerId}) not found! ");

            if (quotation.QuotationLines!.Any(l => l.Id == item.Id))
            {
                var line = quotation.QuotationLines!.First(l => l.Id == item.Id);
                line.SellingPriceId = item.SellingPriceId;
                line.Qty = item.Qty;
                line.ProductId = sellingPrice.ProductId;
                line.Price = sellingPrice.Price;
                line.VAT = sellingPrice.VAT;
                line.Margin = sellingPrice.Margin;
                line.Discount = sellingPrice.Discount;
            }
            else
            {
                quotation.QuotationLines!.Add(new QuotationLine()
                {
                    Id = request.Id,
                    SellingPriceId = item.SellingPriceId,
                    Qty = item.Qty,
                    ProductId = sellingPrice.ProductId,
                    Price = sellingPrice.Price,
                    VAT = sellingPrice.VAT,
                    Margin = sellingPrice.Margin,
                    Discount = sellingPrice.Discount,
                });

            }


        }


    }
}

