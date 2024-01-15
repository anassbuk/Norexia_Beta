using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.VATs.Commands.DeleteVAT;

public record DeleteVATCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeleteVATCommandHandler : IRequestHandler<DeleteVATCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteVATCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteVATCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var vat = await _context.VATs.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (vat == null)
            {
                throw new NotFoundException($"VAT with id ({id}) not found! ");
            }

            vat.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
