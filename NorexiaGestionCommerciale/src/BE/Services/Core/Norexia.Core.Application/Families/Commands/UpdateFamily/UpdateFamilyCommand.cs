using MediatR;

namespace Norexia.Core.Application.Families.Commands.UpdateFamily;

public class UpdateFamilyCommand : IRequest<Guid>
{
    public Guid Id { get; set; }
    public Guid? ParentFamilyId { get; set; }
    public string? Designation { get; set; }
}
