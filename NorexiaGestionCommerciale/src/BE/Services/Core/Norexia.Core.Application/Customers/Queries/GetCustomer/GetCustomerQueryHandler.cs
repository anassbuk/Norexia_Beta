using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.Customers.Queries.GetCustomer;
public record GetCustomerQuery(Guid Id) : IRequest<CustomerDetailsDto?>;
public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDetailsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetCustomerQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerDetailsDto?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers
             .AsNoTracking()
             .ProjectTo<CustomerDetailsDto>(_mapper.ConfigurationProvider)
             .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);
    }
}
