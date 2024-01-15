using AutoMapper;

using MediatR;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Domain.Common.Models;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Queries.GetProduct;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDetailsDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDetailsDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
           .AsNoTracking()
            .Include(p => p.ClassificationInfo)
            .Include(p => p.StorageSupplyInfo)
            .Include(p => p.Images)
            .Include(p => p.Notes)
            .Include(p => p.ProductClassValues)
            .Include(p => p.ProductUnitMeasurements)
            .Include(p => p.SellingPrices)
            .Include(p => p.ProductAvailabilities)
            .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product with id ({request.Id}) not found!");

        var productDto = _mapper.Map<ProductDetailsDto>(product);
        productDto.ProductImages = GetBase64s(product);
        productDto.ProductNotes = _mapper.Map<List<NoteDto>>(product.Notes!.ToList());
        productDto.ProductClassifications = product.ProductClassValues!.Select(p => new ProductClassDto() { ProductClassValueId = p.Id }).ToList();
        productDto.ProductUnits = product.ProductUnitMeasurements!.Select(p => new ProductUnitDto() { ProductUnitMeasurementId = p.Id }).ToList();
        productDto.SellingPrices = _mapper.Map<List<SellingPriceDto>>(product.SellingPrices!.ToList());
        productDto.ProductAvailabilities = product.ProductAvailabilities!.Select(p => new ProductAssignedAvailabilityDto() { ProductAvailabilityId = p.Id }).ToList();

        return productDto;
    }

    private List<FileBase64> GetBase64s(Product product)
    {
        var provider = new FileExtensionContentTypeProvider();
        var base64Files = new List<FileBase64>();
        foreach (var img in product.Images!)
        {
            if (File.Exists(img.Path!))
            {
                string name = Path.GetFileName(img.Path);
                provider.TryGetContentType(name, out string contentType);
                byte[] bytes = File.ReadAllBytes(img.Path!);
                string base64 = Convert.ToBase64String(bytes);

                base64Files.Add(new FileBase64()
                {
                    Name = name,
                    ContentType = contentType,
                    Base64Data = base64
                });
            }
        }
        return base64Files;
    }
}

public record GetProductQuery(Guid Id) : IRequest<ProductDetailsDto?>;
