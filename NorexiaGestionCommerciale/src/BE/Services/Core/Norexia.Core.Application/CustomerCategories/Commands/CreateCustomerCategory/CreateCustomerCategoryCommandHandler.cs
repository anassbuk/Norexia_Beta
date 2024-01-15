using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.CustomerEntities;

namespace Norexia.Core.Application.CustomerCategories.Commands.CreateCustomerCategory;
public class CreateCustomerCategoryCommandHandler : IRequestHandler<CreateCustomerCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateCustomerCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCustomerCategoryCommand request, CancellationToken cancellationToken)
    {
        CustomerCategory category = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        var result = await _context.CustomerCategories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}