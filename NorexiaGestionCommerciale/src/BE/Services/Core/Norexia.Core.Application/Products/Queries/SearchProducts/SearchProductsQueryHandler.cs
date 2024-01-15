using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Products.Queries.GetProducts;

namespace Norexia.Core.Application.Products.Queries.SearchProducts;

public record SearchProductsQuery(string Term) : IRequest<IEnumerable<ProductDto>>;

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchProductsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .AsNoTracking()
            .Where(p => p.Description!.ToLower().Contains(request.Term.ToLower()) ||
                    p.ShortDesignation!.ToLower().Contains(request.Term.ToLower()) ||
                    p.LongDesignation!.ToLower().Contains(request.Term.ToLower()))
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
