using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.CustomerCategories.Commands.DeleteCustomerCategory;
public class DeleteCustomerCategoryCommandHandler : IRequestHandler<DeleteCustomerCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCustomerCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCustomerCategoryCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var category = await _context.CustomerCategories.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (category == null)
            {
                throw new NotFoundException($"Category with id ({id}) not found! ");
            }

            category.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}

public record DeleteCustomerCategoryCommand(IEnumerable<Guid> Ids) : IRequest;