using MediatR;

namespace Norexia.Core.Application.PaymentMeans.Commands.CreatePaymentMean;

public class CreatePaymentMeanCommand : IRequest<Guid>
{
    public string? Name { get; set; }
}
