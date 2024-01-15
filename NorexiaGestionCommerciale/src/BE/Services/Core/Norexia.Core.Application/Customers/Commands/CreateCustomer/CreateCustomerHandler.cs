using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.Exceptions;

namespace Norexia.Core.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerHandler : IRequestHandler<CreateCustomer, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCustomerHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        if (_context.Customers.Any(c => c.Reference == request.Reference))
            throw new NotFoundException($"Client with reference ({request.Reference}) elready exists!");

        var newCustomer = new Customer()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            ClientType = request.ClientType,
            CustomerCategoryId = request.CustomerCategoryId,
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
            //Active = request.Active,
        };
        var customerAddresse = request.CustomerAddresses.Select(w => new CustomerAddress
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
        newCustomer.CustomerAddresses = customerAddresse;
        var result = await _context.Customers.AddAsync(newCustomer, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }
}
