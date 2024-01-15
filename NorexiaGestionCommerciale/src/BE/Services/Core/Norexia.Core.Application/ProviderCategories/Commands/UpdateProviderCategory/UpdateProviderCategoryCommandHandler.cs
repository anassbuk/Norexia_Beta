using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.ProviderCategories.Commands.UpdateProviderCategory;
public class UpdateProviderCategoryCommandHandler : IRequestHandler<UpdateProviderCategoryCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateProviderCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateProviderCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.ProviderCategories.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (category == null)
            throw new NotFoundException($"Category with id ({request.Id}) not found! ");

        category.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}