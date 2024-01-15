using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Families.Queries.GetFamilies;
using Norexia.Core.Application.PriceGroups.Queries.GetDefaultPriceGroup;

namespace Norexia.Core.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<IEnumerable<ProductDto>>;
public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    public GetProductsQueryHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var defaultPriceGroupId = await _mediator.Send(new GetDefaultPriceGroupQuery(), cancellationToken);

        var productDtos = new List<ProductDto>();
        var products = _context.Products
            .AsNoTracking()
            .Where(p => p.IsDeleted == false)
            .Include(p => p.ClassificationInfo)
            .ThenInclude(c => c!.Family).ThenInclude(f => f!.ParentFamily)
            .Include(p => p.StorageSupplyInfo)
            .Include(p => p.SellingPrices);

        foreach (var product in products)
        {
            var productDto = _mapper.Map<ProductDto>(product);
            var defaultSalePrice = product.SellingPrices!.First(s => s.PriceGroupId == defaultPriceGroupId);

            if (product.ClassificationInfo!.Family != null)
            {
                productDto.Family = _mapper.Map<FamilyDto>(product.ClassificationInfo!.Family!.ParentFamily);
                productDto.SubFamily = _mapper.Map<FamilyDto>(product.ClassificationInfo!.Family);
            }

            productDto.DefaultSalePrice_TaxeExcluded = defaultSalePrice.Price;
            productDto.DefaultSalePrice_TaxeIncluded = defaultSalePrice.Price + (defaultSalePrice.Price * ((double)defaultSalePrice.VAT! / 100));
            productDto.MultiPrice = product.SellingPrices!.Count > 1;

            productDtos.Add(productDto);
        }

        return productDtos;
    }
}
