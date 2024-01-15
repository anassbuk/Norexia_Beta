using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ProviderCategories.Commands.DeleteProviderCategory;
public class DeleteProviderCategoryCommandHandler : IRequestHandler<DeleteProviderCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteProviderCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteProviderCategoryCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var category = await _context.ProviderCategories.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (category == null)
            {
                throw new NotFoundException($"Category with id ({id}) not found! ");
            }

            category.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeleteProviderCategoryCommand(IEnumerable<Guid> Ids) : IRequest;
