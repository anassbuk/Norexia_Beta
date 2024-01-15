using MediatR;

namespace Norexia.Core.Application.VATs.Commands.CreateVAT;

public class CreateVATCommand : IRequest<Guid>
{
    public decimal? Value { get; set; }
}
