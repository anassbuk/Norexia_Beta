using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Commands.CreateProduct;
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IFileServices _fileServices;
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IFileServices fileServices, IApplicationDbContext context)
    {
        _fileServices = fileServices;
        _context = context;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (_context.Products.Any(p => p.Reference == request.Reference))
            throw new NotFoundException($"Product with reference ({request.Reference}) elready exists!");

        var newProduct = new Product()
        {
            Reference = request.Reference,
            Id = Guid.NewGuid(),
            ShortDesignation = request.ShortDesignation,
            LongDesignation = request.LongDesignation,
            Description = request.Description,
            Type = request.Type,
            Action = request.Action,
            Barcode = request.Barcode,
            ClassificationInfo = request.ClassificationInfo!,
            PurchaseInfo = request.PurchaseInfo!,
            SellInfo = request.SellInfo!,
            UnitInfo = request.UnitInfo!,
            StorageSupplyInfo = request.StorageSupplyInfo!,
        };

        newProduct.Images = await AddPictures(request, newProduct.Id, cancellationToken).ConfigureAwait(false);
        newProduct.Notes = AddNotes(request);
        newProduct.ProductClassValues = AddClassifications(request);
        newProduct.ProductUnitMeasurements = AddUnits(request);
        newProduct.SellingPrices = AddSellingPrices(request);
        newProduct.ProductAvailabilities = AddProductAvailabilities(request);

        var result = await _context.Products.AddAsync(newProduct, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;

    }

    private static List<ProductNote>? AddNotes(CreateProductCommand request)
    {
        if (request.ProductNotes is null || request.ProductNotes!.Count == 0)
            return null;


        var notes = new List<ProductNote>();
        foreach (var item in request.ProductNotes)
        {
            notes.Add(new ProductNote()
            {
                Created = item.Created,

                //TODO : change to current authenticated user
                CreatedBy = item.CreatedBy,

                Note = item.Note
            });
        }

        return notes;
    }

    private async Task<List<ProductImage>?> AddPictures(CreateProductCommand request, Guid productId, CancellationToken cancellation = default)
    {

        if (request.ProductImages is null || request.ProductImages!.Count == 0)
            return null;

        var productImages = new List<ProductImage>();
        foreach (var item in request.ProductImages)
        {
            var path = await _fileServices.StoreProductPictureAsync(productId, item.Name!, item.Base64Data!, cancellation);

            productImages.Add(new ProductImage()
            {
                Path = path,
            });
        }

        return productImages;
    }

    private List<ClassValue>? AddClassifications(CreateProductCommand request)
    {
        if (request.ProductClassifications is null || request.ProductClassifications!.Count == 0)
            return null;


        var classifications = new List<ClassValue>();
        foreach (var item in request.ProductClassifications)
        {
            var classValue = new ClassValue()
            {
                Id = item.ProductClassValueId,
            };
            _context.ClassValues.Attach(classValue);
            classifications.Add(classValue);
        }

        return classifications;
    }

    private List<UnitMeasurement>? AddUnits(CreateProductCommand request)
    {
        if (request.ProductUnits is null || request.ProductUnits!.Count == 0)
            return null;


        var units = new List<UnitMeasurement>();
        foreach (var item in request.ProductUnits)
        {
            var unitMeasurement = new UnitMeasurement()
            {
                Id = item.ProductUnitMeasurementId,
            };
            _context.UnitMeasurements.Attach(unitMeasurement);
            units.Add(unitMeasurement);
        }

        return units;
    }

    private List<SellingPrice>? AddSellingPrices(CreateProductCommand request)
    {
        if (request.SellingPrices is null || request.SellingPrices!.Count == 0)
            return null;

        var sellingPrices = new List<SellingPrice>();
        foreach (var item in request.SellingPrices)
        {
            if (!_context.PriceGroups.Any(f => f.Id == item.PriceGroupId))
                throw new NotFoundException($"Price Group with id ({item.PriceGroupId}) not found! ");

            sellingPrices.Add(new SellingPrice()
            {
                PriceGroupId = item.PriceGroupId,
                Price = item.Price,
                Discount = item.Discount,
                Margin = item.Margin,
                VAT = item.VAT,
            });
        }

        return sellingPrices;
    }


    private List<ProductAvailability>? AddProductAvailabilities(CreateProductCommand request)
    {
        if (request.ProductAvailabilities is null || request.ProductAvailabilities!.Count == 0)
            return null;

        var availabilities = new List<ProductAvailability>();
        foreach (var item in request.ProductAvailabilities)
        {
            var productAvailability = new ProductAvailability()
            {
                Id = item.ProductAvailabilityId
            };
            _context.ProductAvailabilities.Attach(productAvailability);
            availabilities.Add(productAvailability);
        }

        return availabilities;
    }
}