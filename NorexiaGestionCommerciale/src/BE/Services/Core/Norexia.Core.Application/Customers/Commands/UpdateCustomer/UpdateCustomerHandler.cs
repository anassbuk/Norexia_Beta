using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Customers.Commands.UpdateCustomer;
public class UpdateCustomerHandler : IRequestHandler<UpdateCustomer, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdateCustomerHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(UpdateCustomer request, CancellationToken cancellationToken)
    {
        var customer = await _context.Customers
                                .Include(c => c.CustomerAddresses)
                                    .SingleOrDefaultAsync(t => t.Id == request.Id);
        if (customer == null)
        {
            throw new NotFoundException($"Customer with id ({request.Id}) not found! ");
        }

        customer.ClientType = request.ClientType;
        customer.CustomerCategoryId = request.CustomerCategoryId;
        customer.LastName = request.LastName;
        customer.FirstName = request.FirstName;
        customer.SocialReason = request.SocialReason;
        customer.ICE = request.ICE;
        customer.Tel = request.Tel;
        customer.Fax = request.Fax;
        customer.Mobile = request.Mobile;
        customer.Email = request.Email;
        customer.WebSite = request.WebSite;
        customer.Function = request.Function;
        customer.CompanyEmail = request.CompanyEmail;
        customer.CompanyFax = request.CompanyFax;
        customer.CompanyTel = request.CompanyTel;
        customer.Location = request.Location;

        HandleCustomerAddresses(request, customer);

        await _context.SaveChangesAsync(cancellationToken);
        return customer.Id;
    }

    private void HandleCustomerAddresses(UpdateCustomer request, Customer customer)
    {
        var AddressesToRemove = customer.CustomerAddresses!
            .Where(ca => !request.CustomerAddresses!.Any(a => a.Id == ca.Id));

        foreach (var address in AddressesToRemove.ToList())
            customer.CustomerAddresses!.Remove(address);

        foreach (var item in request.CustomerAddresses!)
        {
            if (customer.CustomerAddresses!.Any(c => c.Id == item.Id))
            {
                var address = customer.CustomerAddresses!.First(c => c.Id == item.Id);
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
                customer.CustomerAddresses!.Add(new CustomerAddress()
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
