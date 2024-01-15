using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Products.Commands.DeleteProduct;
internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var product = await _context.Products.SingleOrDefaultAsync(t => t.Id == id);

            if (product == null)
            {
                throw new NotFoundException($"Product with id ({id}) not found! ");
            }

            product.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}


public record DeleteProductCommand(IEnumerable<Guid> Ids) : IRequest;
