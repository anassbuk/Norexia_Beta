using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Customers.Queries.GetCustomer;

namespace Norexia.Core.Application.Customers.Queries.SearchCustomers;

public record SearchCustomerQueryByName(string Name) : IRequest<IEnumerable<CustomerDetailsDto>>;

public class SearchCustomerByName : IRequestHandler<SearchCustomerQueryByName, IEnumerable<CustomerDetailsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public SearchCustomerByName(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CustomerDetailsDto>> Handle(SearchCustomerQueryByName request, CancellationToken cancellationToken)
    {
        return await _context.Customers
         .AsNoTracking()
         .Where(t =>
                t.FirstName!.Contains(request.Name, StringComparison.InvariantCultureIgnoreCase)
                || t.LastName!.Contains(request.Name, StringComparison.InvariantCultureIgnoreCase))
         .ProjectTo<CustomerDetailsDto>(_mapper.ConfigurationProvider)
         .ToListAsync(cancellationToken);
    }
}
