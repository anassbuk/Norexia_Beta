using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.Products.Commands.CreateProduct;

namespace Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;
internal class GetDeliveryLinesQueryHandler : IRequestHandler<GetDeliveryLinesQuery, IEnumerable<DeliveryLineDto>?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDeliveryLinesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DeliveryLineDto>?> Handle(GetDeliveryLinesQuery request, CancellationToken cancellationToken)
    {
        List<DeliveryLineDto> deliveryLineDtos = new();
        var lines = await _context.DeliveryLines
            .AsNoTracking()
            .Where(t => t.DeliveryId == request.Id)
            .Include(l => l.Product)
            .ThenInclude(p => p!.SellingPrices)
            .ToListAsync(cancellationToken);

        foreach (var line in lines)
        {
            DeliveryLineDto lineDto = _mapper.Map<DeliveryLineDto>(line);
            lineDto.TotalPriceExcludingTax = line.Qty * (line.UnitPrice - (line.UnitPrice * (((double?)line.Discount ?? 0) / 100)));
            var totalVATPrice = lineDto.TotalPriceExcludingTax * (((double?)line.VAT ?? 0) / 100);
            lineDto.TotalPriceIncludingTax = lineDto.TotalPriceExcludingTax + totalVATPrice;
            lineDto.ShortDesignation = line.Product!.ShortDesignation;
            lineDto.Reference = line.Product!.Reference;
            lineDto.SellingPrices = _mapper.Map<ICollection<SellingPriceDto>>(line.Product!.SellingPrices);

            deliveryLineDtos.Add(lineDto);
        }

        return deliveryLineDtos;
    }
}
public record GetDeliveryLinesQuery(Guid Id) : IRequest<IEnumerable<DeliveryLineDto>?>;
