using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Application.Currencies.Commands.CreateCurrency;

public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, Guid>
{

    private readonly IApplicationDbContext _context;
    public CreateCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        Currency currency = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
        };

        var result = await _context.Currencies.AddAsync(currency);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
