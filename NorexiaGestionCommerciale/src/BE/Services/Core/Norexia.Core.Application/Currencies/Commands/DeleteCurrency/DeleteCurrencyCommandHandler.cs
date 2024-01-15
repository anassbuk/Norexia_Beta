using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Currencies.Commands.DeleteCurrency;

public record DeleteCurrencyCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCurrencyCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var currency = await _context.Currencies.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (currency == null)
            {
                throw new NotFoundException($"Currency with id ({id}) not found! ");
            }

            currency.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
