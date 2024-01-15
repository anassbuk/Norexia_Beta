using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Application.Providers.Commands.UpdateProvider;

public class UpdateProviderHandler : IRequestHandler<UpdateProvider, Guid>
{
    private readonly IApplicationDbContext _context;

    public UpdateProviderHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(UpdateProvider request, CancellationToken cancellationToken)
    {
        var provider = await _context.Providers
                                .Include(p => p.ProviderAddresses)
                                .SingleOrDefaultAsync(t => t.Id == request.Id);
        if (provider == null)
        {
            throw new NotFoundException($"Provider with id ({request.Id}) not found! ");
        }

        provider.ProviderType = request.ProviderType;
        provider.ProviderCategoryId = request.ProviderCategoryId;
        provider.LastName = request.LastName;
        provider.FirstName = request.FirstName;
        provider.SocialReason = request.SocialReason;
        provider.ICE = request.ICE;
        provider.Tel = request.Tel;
        provider.Fax = request.Fax;
        provider.Mobile = request.Mobile;
        provider.Email = request.Email;
        provider.WebSite = request.WebSite;
        provider.Function = request.Function;
        provider.CompanyEmail = request.CompanyEmail;
        provider.CompanyFax = request.CompanyFax;
        provider.CompanyTel = request.CompanyTel;
        provider.Location = request.Location;

        HandleProviderAddresses(request, provider);

        await _context.SaveChangesAsync(cancellationToken);
        return provider.Id;
    }
    private void HandleProviderAddresses(UpdateProvider request, Provider provider)
    {
        var AddressesToRemove = provider.ProviderAddresses!
            .Where(ca => !request.ProviderAddresses!.Any(a => a.Id == ca.Id));

        foreach (var address in AddressesToRemove.ToList())
            provider.ProviderAddresses!.Remove(address);


        foreach (var item in request.ProviderAddresses!)
        {
            if (provider.ProviderAddresses!.Any(c => c.Id == item.Id))
            {
                var address = provider.ProviderAddresses!.First(c => c.Id == item.Id);
                address.StreetAdress = item.StreetAdress;
                address.City = item.City;
                address.Active = item.Active;
                address.Region = item.Region;
                address.Localisation = item.Localisation;
                address.Complement = item.Complement;
                address.ZipCode = item.ZipCode;
                address.AddressType = item.AddressType;
            }
            else
            {
                provider.ProviderAddresses!.Add(new ProviderAddress()
                {
                    StreetAdress = item.StreetAdress,
                    City = item.City,
                    Active = item.Active,
                    Region = item.Region,
                    Localisation = item.Localisation,
                    Complement = item.Complement,
                    ZipCode = item.ZipCode,
                    AddressType = item.AddressType
                });
            }
        }
    }

}
