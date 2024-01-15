using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.PaymentMeans.Commands.DeletePaymentMean;

public class DeletePaymentMeanCommandHandler : IRequestHandler<DeletePaymentMeanCommand>
{
    private readonly IApplicationDbContext _context;

    public DeletePaymentMeanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeletePaymentMeanCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var mean = await _context.PaymentMeans.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (mean == null)
            {
                throw new NotFoundException($"Payment mean with id ({id}) not found! ");
            }

            mean.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record DeletePaymentMeanCommand(IEnumerable<Guid> Ids) : IRequest;
