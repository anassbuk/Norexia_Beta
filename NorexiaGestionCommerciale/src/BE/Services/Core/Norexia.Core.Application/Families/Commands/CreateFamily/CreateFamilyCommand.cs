using MediatR;

namespace Norexia.Core.Application.Families.Commands.CreateFamily;

public class CreateFamilyCommand : IRequest<Guid>
{
    public Guid? ParentFamilyId { get; set; }
    public string? Designation { get; set; }
}
