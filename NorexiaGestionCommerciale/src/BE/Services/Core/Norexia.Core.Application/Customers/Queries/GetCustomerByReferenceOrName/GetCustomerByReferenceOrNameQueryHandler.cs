using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Customers.Queries.GetCustomer;

namespace Norexia.Core.Application.Customers.Queries.GetCustomerByReferenceOrName;
public record GetCustomerByReferenceOrNameQuery(string term) : IRequest<CustomerDetailsDto?>;
public class GetCustomerByReferenceOrNameQueryHandler : IRequestHandler<GetCustomerByReferenceOrNameQuery, CustomerDetailsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetCustomerByReferenceOrNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerDetailsDto?> Handle(GetCustomerByReferenceOrNameQuery request, CancellationToken cancellationToken)
    {
        return await _context.Customers
             .AsNoTracking()
             .ProjectTo<CustomerDetailsDto>(_mapper.ConfigurationProvider)
             .FirstOrDefaultAsync(t => t.Reference!.ToLower() == request.term.ToLower() ||
                                        (t.SocialReason != null && t.SocialReason!.ToLower() == request.term.ToLower()) ||
                                          t.LastName!.ToLower() == request.term.ToLower(), cancellationToken);
    }
}
