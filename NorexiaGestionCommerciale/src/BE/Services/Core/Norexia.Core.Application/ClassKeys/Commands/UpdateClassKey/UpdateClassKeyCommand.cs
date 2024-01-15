using MediatR;

namespace Norexia.Core.Application.ClassKeys.Commands.UpdateClassKey;

public class UpdateClassKeyCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string? Key { get; set; }
}
