using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.VATs.Commands.UpdateVAT;

public class UpdateVATCommandHandler : IRequestHandler<UpdateVATCommand, Guid>
{
    private IApplicationDbContext _context;
    public UpdateVATCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateVATCommand request, CancellationToken cancellationToken)
    {
        var vat = await _context.VATs.SingleOrDefaultAsync(f => f.Id == request.Id, cancellationToken: cancellationToken);

        if (vat == null)
            throw new NotFoundException($"VAT with id ({request.Id}) not found!");

        vat.Value = request.Value ?? 0;

        await _context.SaveChangesAsync(cancellationToken);

        return vat.Id;
    }
}
