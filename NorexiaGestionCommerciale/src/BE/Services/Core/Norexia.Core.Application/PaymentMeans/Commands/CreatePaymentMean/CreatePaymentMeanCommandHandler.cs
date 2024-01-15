using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.SaleOrderEntities;

namespace Norexia.Core.Application.PaymentMeans.Commands.CreatePaymentMean;

public class CreatePaymentMeanCommandHandler : IRequestHandler<CreatePaymentMeanCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreatePaymentMeanCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePaymentMeanCommand request, CancellationToken cancellationToken)
    {
        PaymentMean mean = new()
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };

        var result = await _context.PaymentMeans.AddAsync(mean, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
