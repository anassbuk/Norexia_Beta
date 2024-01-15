using MediatR;

namespace Norexia.Core.Application.ClassValues.Commands.UpdateClassValue;
public class UpdateClassValueCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid ClassId { get; set; }
    public string? Value { get; set; }
}
