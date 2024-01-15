using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Providers.Commands.ActivateProvider;
public class ActivateProviderCommandHandler : IRequestHandler<ActivateProviderCommand>
{
    private readonly IApplicationDbContext _context;
    public ActivateProviderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(ActivateProviderCommand request, CancellationToken cancellationToken)
    {
        foreach (var id in request.Ids)
        {
            var provider = await _context.Providers.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);

            if (provider == null)
            {
                throw new NotFoundException($"Provider with id ({id}) not found! ");
            }

            provider.Active = !provider.Active;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
public record ActivateProviderCommand(IEnumerable<Guid> Ids) : IRequest;
