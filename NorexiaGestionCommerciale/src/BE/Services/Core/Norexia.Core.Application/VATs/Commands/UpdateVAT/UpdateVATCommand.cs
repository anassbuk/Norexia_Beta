using MediatR;

namespace Norexia.Core.Application.VATs.Commands.UpdateVAT;

public class UpdateVATCommand : IRequest<Guid>
{
    public Guid? Id { get; set; }
    public decimal? Value { get; set; }
}
