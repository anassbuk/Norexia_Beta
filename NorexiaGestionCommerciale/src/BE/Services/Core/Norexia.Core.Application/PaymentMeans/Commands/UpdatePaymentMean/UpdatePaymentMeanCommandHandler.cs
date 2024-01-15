using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.PaymentMeans.Commands.UpdatePaymentMean;

public class UpdatePaymentMeanCommandHandler : IRequestHandler<UpdatePaymentMeanCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdatePaymentMeanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdatePaymentMeanCommand request, CancellationToken cancellationToken)
    {
        var mean = await _context.PaymentMeans.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (mean == null)
            throw new NotFoundException($"Payment mean with id ({request.Id}) not found! ");

        mean.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return mean.Id;
    }
}
