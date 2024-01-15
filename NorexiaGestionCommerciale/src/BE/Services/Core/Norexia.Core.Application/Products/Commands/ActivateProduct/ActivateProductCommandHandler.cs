using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Products.Commands.ActivateProduct;


public class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand>
{
    private readonly IApplicationDbContext _context;
    public ActivateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var product = await _context.Products.SingleOrDefaultAsync(t => t.Id == id);

            if (product == null)
            {
                throw new NotFoundException($"Product with id ({id}) not found! ");
            }

            product.Active = !product.Active;
            _context.Products.Update(product);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

public record ActivateProductCommand(IEnumerable<Guid> Ids) : IRequest;
