using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.ProviderCategories.Queries.GetProviderCategories;
public record GetProviderCategoriesQuery : IRequest<IEnumerable<ProviderCategoryDto>>;
public class GetProviderCategoriesQueryHandler : IRequestHandler<GetProviderCategoriesQuery, IEnumerable<ProviderCategoryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetProviderCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<ProviderCategoryDto>> Handle(GetProviderCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProviderCategories
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .ProjectTo<ProviderCategoryDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
