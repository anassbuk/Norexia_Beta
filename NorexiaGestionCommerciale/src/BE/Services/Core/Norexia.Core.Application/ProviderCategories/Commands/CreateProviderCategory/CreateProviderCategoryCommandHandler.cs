using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Application.ProviderCategories.Commands.CreateProviderCategory;
public class CreateProviderCategoryCommandHandler : IRequestHandler<CreateProviderCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateProviderCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProviderCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new ProviderCategory()
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        var result = await _context.ProviderCategories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
