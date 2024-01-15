using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Customers.Queries.GetCustomer;

namespace Norexia.Core.Application.Customers.Queries.SearchCustomers;

public record SearchCustomerQueryByCategorie(Guid CategorieId) : IRequest<IEnumerable<CustomerDetailsDto>>;
public class SearchCustomerByCategory : IRequestHandler<SearchCustomerQueryByCategorie, IEnumerable<CustomerDetailsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public SearchCustomerByCategory(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CustomerDetailsDto>> Handle(SearchCustomerQueryByCategorie request, CancellationToken cancellationToken)
    {
        return await _context.Customers
         .AsNoTracking()
         .Where(t => t.CustomerCategoryId == request.CategorieId)
         .ProjectTo<CustomerDetailsDto>(_mapper.ConfigurationProvider)
         .ToListAsync(cancellationToken);
    }
}
