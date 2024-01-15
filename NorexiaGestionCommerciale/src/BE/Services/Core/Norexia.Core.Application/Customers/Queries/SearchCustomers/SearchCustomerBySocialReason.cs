using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Customers.Queries.GetCustomer;

namespace Norexia.Core.Application.Customers.Queries.SearchCustomers;

public record SearchCustomerQueryBySocialReason(string raisonsocial) : IRequest<IEnumerable<CustomerDetailsDto>>;
public class SearchCustomerBySocialReason : IRequestHandler<SearchCustomerQueryBySocialReason, IEnumerable<CustomerDetailsDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public SearchCustomerBySocialReason(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CustomerDetailsDto>> Handle(SearchCustomerQueryBySocialReason request, CancellationToken cancellationToken)
    {
        return await _context.Customers
        .AsNoTracking()
        .Where(t => t.SocialReason!.ToLower().Contains(request.raisonsocial.ToLower()))
        .ProjectTo<CustomerDetailsDto>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
    }
}
