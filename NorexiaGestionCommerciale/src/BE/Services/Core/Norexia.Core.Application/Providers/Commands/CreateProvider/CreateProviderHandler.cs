using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Application.Providers.Commands.CreateProvider;

public class CreateProviderHandler : IRequestHandler<CreateProvider, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateProviderHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateProvider request, CancellationToken cancellationToken)
    {

        if (_context.Providers.Any(p => p.Reference == request.Reference))
            throw new NotFoundException($"Provider with reference ({request.Reference}) elready exists!");

        var newProvider = new Provider()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            ProviderType = request.ProviderType,
            ProviderCategoryId = request.ProviderCategoryId,
            LastName = request.LastName,
            FirstName = request.FirstName,
            SocialReason = request.SocialReason,
            ICE = request.ICE,
            Tel = request.Tel,
            Fax = request.Fax,
            Mobile = request.Mobile,
            Email = request.Email,
            WebSite = request.WebSite,
            Function = request.Function,
            CompanyEmail = request.CompanyEmail,
            CompanyFax = request.CompanyFax,
            CompanyTel = request.CompanyTel,
            Location = request.Location,
        };
        var providerAddresse = request.ProviderAddresses.Select(w => new ProviderAddress
        {
            AddressType = w.AddressType,
            StreetAdress = w.StreetAdress,
            ZipCode = w.ZipCode,
            City = w.City,
            Region = w.Region,
            Localisation = w.Localisation,
            Complement = w.Complement,
            Active = w.Active
        }).ToList();
        newProvider.ProviderAddresses = providerAddresse;
        var result = await _context.Providers.AddAsync(newProvider, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }
}
