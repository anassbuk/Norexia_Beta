using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Customers.Queries.GetCustomers;

public record GetCustomersQuery : IRequest<IEnumerable<CustomerDto>>;
public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCustomersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers
                        .AsNoTracking()
                        .Where(c => !c.IsDeleted)
                        .Include(c => c.CustomerCategory)
                        .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
