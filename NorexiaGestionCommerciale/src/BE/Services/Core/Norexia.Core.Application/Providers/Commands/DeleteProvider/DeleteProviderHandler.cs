using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Providers.Commands.DeleteProvider;

public record DeleteProvider(IEnumerable<Guid> Ids) : IRequest;
internal class DeleteProviderHandler : IRequestHandler<DeleteProvider>
{
    private readonly IApplicationDbContext _context;

    public DeleteProviderHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteProvider request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var provider = await _context.Providers.SingleOrDefaultAsync(t => t.Id == id);
            if (provider == null)
            {
                throw new NotFoundException($"Provider with id {id} not found! ");
            }

            provider.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
