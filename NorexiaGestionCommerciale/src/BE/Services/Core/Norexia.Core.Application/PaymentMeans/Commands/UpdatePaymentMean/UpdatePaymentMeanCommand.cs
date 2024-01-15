using MediatR;

namespace Norexia.Core.Application.PaymentMeans.Commands.UpdatePaymentMean;

public class UpdatePaymentMeanCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}
