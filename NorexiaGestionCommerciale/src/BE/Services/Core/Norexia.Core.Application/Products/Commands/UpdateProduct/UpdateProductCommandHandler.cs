using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Commands.UpdateProduct;

internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileServices _fileServices;

    public UpdateProductCommandHandler(IApplicationDbContext context, IFileServices fileServices)
    {
        _context = context;
        _fileServices = fileServices;
    }

    public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        //TODO: It needs to be refactored
        var product = await _context.Products
                                .Include(p => p.Images)
                                .Include(p => p.Notes)
                                .Include(p => p.ProductClassValues)
                                .Include(p => p.ProductUnitMeasurements)
                                .Include(p => p.SellingPrices)
                                .Include(p => p.ProductAvailabilities)
                                .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken: cancellationToken);

        if (product == null)
            throw new NotFoundException($"Product with id ({request.Id}) not found! ");

        await HandlePictures(request, product, cancellationToken);
        HandleNotes(request, product);
        HandleClassifications(request, product);
        HandleUnits(request, product);
        HandleSellingPrices(request, product);
        await HandleProductAvailabilities(request, product, cancellationToken);

        product.Description = request.Description;
        product.ShortDesignation = request.ShortDesignation;
        product.LongDesignation = request.LongDesignation;
        product.Type = request.Type;
        product.Action = request.Action;
        product.Barcode = request.Barcode;
        product.ClassificationInfo = request.ClassificationInfo!;
        product.PurchaseInfo = request.PurchaseInfo!;
        product.SellInfo = request.SellInfo!;
        product.UnitInfo = request.UnitInfo!;
        product.StorageSupplyInfo = request.StorageSupplyInfo!;

        await _context.SaveChangesAsync(cancellationToken);

        return product.Id;
    }

    private async Task HandlePictures(UpdateProductCommand request, Product product, CancellationToken cancellation = default)
    {
        foreach (var img in product.Images!)
        {
            product.Images.Remove(img);
            if (File.Exists(img.Path))
                File.Delete(img.Path!);
        }

        foreach (var item in request.ProductImages!)
        {
            var path = await _fileServices.StoreProductPictureAsync(product.Id, item.Name!, item.Base64Data!, cancellation);

            var pImg = new ProductImage()
            {
                Path = path,
            };

            product.Images!.Add(pImg);
        }
    }

    private static void HandleNotes(UpdateProductCommand request, Product product)
    {
        foreach (var item in request.ProductNotes!)
        {
            var note = new ProductNote()
            {
                ProductId = product.Id,
                Note = item.Note,

                Created = item.Created,

                //TODO : change to current authenticated user
                CreatedBy = item.CreatedBy,
            };

            product.Notes!.Add(note);
        }
    }

    private void HandleClassifications(UpdateProductCommand request, Product product)
    {
        var ClassificationsToRemove = product.ProductClassValues!
            .Where(pcv => !request.ProductClassifications!.Any(pc => pc.ProductClassValueId == pcv.Id));

        foreach (var Classification in ClassificationsToRemove)
            product.ProductClassValues!.Remove(Classification);

        var ClassificationsToAdd = request.ProductClassifications!.Where(
            pc => !product.ProductClassValues!
                    .Any(pcv => pcv.Id == pc.ProductClassValueId));

        foreach (var item in ClassificationsToAdd)
        {
            var classValue = new ClassValue()
            {
                Id = item.ProductClassValueId,
            };
            _context.ClassValues.Attach(classValue);
            product.ProductClassValues!.Add(classValue);
        }
    }

    private void HandleUnits(UpdateProductCommand request, Product product)
    {
        var unitsToRemove = product.ProductUnitMeasurements!
            .Where(pum => !request.ProductUnits!.Any(pu => pu.ProductUnitMeasurementId == pum.Id));

        foreach (var unit in unitsToRemove)
            product.ProductUnitMeasurements!.Remove(unit);

        var unitsToAdd = request.ProductUnits!.Where(
            pu => !product.ProductUnitMeasurements!
                    .Any(pum => pum.Id == pu.ProductUnitMeasurementId));

        foreach (var item in unitsToAdd)
        {
            var unitMeasurement = new UnitMeasurement()
            {
                Id = item.ProductUnitMeasurementId,
            };
            _context.UnitMeasurements.Attach(unitMeasurement);
            product.ProductUnitMeasurements!.Add(unitMeasurement);
        }
    }

    private void HandleSellingPrices(UpdateProductCommand request, Product product)
    {
        var sellingPricesToRemove = product.SellingPrices!
            .Where(sp => !request.SellingPrices!.Any(p => p.PriceGroupId == sp.PriceGroupId));

        foreach (var sellingPrice in sellingPricesToRemove)
            product.SellingPrices!.Remove(sellingPrice);

        foreach (var item in request.SellingPrices!)
        {
            SellingPrice sellingPrice;
            if (product.SellingPrices!.Any(p => p.PriceGroupId == item.PriceGroupId))
            {
                sellingPrice = product.SellingPrices!.First(p => p.PriceGroupId == item.PriceGroupId);
                sellingPrice.Price = item.Price;
                sellingPrice.Discount = item.Discount;
                sellingPrice.Margin = item.Margin;
                sellingPrice.VAT = item.VAT;
            }
            else
            {
                if (!_context.PriceGroups.Any(f => f.Id == item.PriceGroupId))
                    throw new NotFoundException($"Price Group with id ({item.PriceGroupId}) not found! ");

                product.SellingPrices!.Add(new SellingPrice()
                {
                    PriceGroupId = item.PriceGroupId,
                    Price = item.Price,
                    Discount = item.Discount,
                    Margin = item.Margin,
                    VAT = item.VAT,
                });
            }
        }
    }

    private async Task HandleProductAvailabilities(UpdateProductCommand request, Product product, CancellationToken cancellationToken)
    {
        product.ProductAvailabilities!.Clear();
        if (request.ProductAvailabilities is null || request.ProductAvailabilities!.Count == 0)
            return;

        var availabilities = new List<ProductAvailability>();
        foreach (var item in request.ProductAvailabilities)
        {
            //var productAvailability = new ProductAvailability()
            //{
            //    Id = item.ProductAvailabilityId
            //};

            //if (_context.Entry(productAvailability).State == EntityState.Detached)
            //    _context.ProductAvailabilities.Attach(productAvailability);

            //TODO : this is a quick fix for the above not working code
            var productAvailability = await _context.ProductAvailabilities.FindAsync(item.ProductAvailabilityId, cancellationToken);

            if (productAvailability != null)
                product.ProductAvailabilities!.Add(productAvailability);
        }
    }

}
