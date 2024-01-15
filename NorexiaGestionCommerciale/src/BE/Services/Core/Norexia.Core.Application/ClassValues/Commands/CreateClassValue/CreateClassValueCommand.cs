using MediatR;

namespace Norexia.Core.Application.ClassValues.Commands.CreateClassValue;

public class CreateClassValueCommand : IRequest<Guid>
{
    public Guid ClassId { get; set; }
    public string? Value { get; set; }
}
