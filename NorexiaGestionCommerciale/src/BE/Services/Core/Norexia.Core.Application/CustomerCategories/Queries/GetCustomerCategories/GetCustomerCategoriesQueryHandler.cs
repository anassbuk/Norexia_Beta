using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.CustomerCategories.Queries.GetCustomerCategories;
public record GetCustomerCategoriesQuery : IRequest<IEnumerable<CustomerCategoryDto>>;
public class GetCustomerCategoriesQueryHandler : IRequestHandler<GetCustomerCategoriesQuery, IEnumerable<CustomerCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetCustomerCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerCategoryDto>> Handle(GetCustomerCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.CustomerCategories
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .ProjectTo<CustomerCategoryDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
