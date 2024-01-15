using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Quotations.Commands.DeletQuotation;

internal class DeleteQuotationommandHandler : IRequestHandler<DeletQuotationommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteQuotationommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletQuotationommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var quotation = _context.Quotations.Find(id);
            if (quotation == null)
            {
                throw new NotFoundException($"Quotation with id ({id}) not found! ");
            }
            quotation.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeletQuotationommand(IEnumerable<Guid> Ids) : IRequest;