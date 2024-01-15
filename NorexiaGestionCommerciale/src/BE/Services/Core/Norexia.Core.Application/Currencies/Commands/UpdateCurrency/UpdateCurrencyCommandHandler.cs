using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Currencies.Commands.UpdateCurrency;

public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var currency = await _context.Currencies.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (currency == null)
            throw new NotFoundException($"Currency with id ({request.Id}) not found!");

        currency.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return currency.Id;
    }
}
