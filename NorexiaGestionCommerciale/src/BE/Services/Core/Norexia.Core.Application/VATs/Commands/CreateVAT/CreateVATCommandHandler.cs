using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Application.VATs.Commands.CreateVAT;

public class CreateVATCommandHandler : IRequestHandler<CreateVATCommand, Guid>
{

    private readonly IApplicationDbContext _context;
    public CreateVATCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateVATCommand request, CancellationToken cancellationToken)
    {
        VAT vat = new()
        {
            Id = Guid.NewGuid(),
            Value = request.Value ?? 0,
        };

        var result = await _context.VATs.AddAsync(vat);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
}
