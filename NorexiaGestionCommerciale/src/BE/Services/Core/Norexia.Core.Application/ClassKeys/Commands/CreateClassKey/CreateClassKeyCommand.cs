using MediatR;

namespace Norexia.Core.Application.ClassKeys.Commands.CreateClassKey;

public class CreateClassKeyCommand : IRequest<Guid>
{
    public string? Key { get; set; }
}
