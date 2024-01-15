using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Deliveries.Queries.GetDeliveries;
public record GetDeliveriesQuery : IRequest<IEnumerable<DeliveryDto>>;
public class GetDeliveriesQueryHandler : IRequestHandler<GetDeliveriesQuery, IEnumerable<DeliveryDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetDeliveriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DeliveryDto>> Handle(GetDeliveriesQuery request, CancellationToken cancellationToken)
    {
        var deliveries = await _context.Deliveries
                                .AsNoTracking()
                                .Where(c => c.IsDeleted == false)
                                .Include(c => c.Customer)
                                .ThenInclude(c => c!.CustomerAddresses)
                                .Include(c => c.SaleOrder)
                                .Include(c => c.Deliverer)
                                .Include(c => c.Invoice)
                                .Include(c => c.DeliveryLines)
                                .ToListAsync(cancellationToken);

        var deliveriesDto = new List<DeliveryDto>();

        foreach (var delivery in deliveries)
        {
            var deliveryDto = _mapper.Map<DeliveryDto>(delivery);

            if (delivery.DeliveryOrigin == DeliveryOrigin.SaleOrder)
                deliveryDto.SaleOrderRef = delivery.SaleOrder!.Reference;

            if (delivery.DeliveryOrigin == DeliveryOrigin.Facture)
                deliveryDto.InvoiceRef = delivery.Invoice!.Reference;

            deliveryDto.CustomerRef = $"{delivery.Customer!.Reference} - {(delivery.Customer!.ClientType == ClientType.Particular ? $"{delivery.Customer!.FirstName}, {delivery.Customer!.LastName}" : delivery.Customer!.SocialReason)}";
            deliveryDto.DelivererRef = $"{delivery.Deliverer!.Reference} - {(delivery.Deliverer!.Type == DelivererType.Internal ? $"{delivery.Customer!.FirstName}, {delivery.Customer!.LastName}" : delivery.Customer!.SocialReason)}";

            deliveryDto.TotalPriceExcludingVAT = delivery.DeliveryLines!.Select(line => line.Qty * (line.UnitPrice - (line.UnitPrice * (((double?)line.Discount ?? 0) / 100)))).Sum();

            var taxPrice = delivery.DeliveryLines!.Select(l => l.Qty * l.UnitPrice * (((double?)l.VAT ?? 0) / 100)).Sum();

            deliveryDto.TotalPriceIncludingVAT = deliveryDto.TotalPriceExcludingVAT + taxPrice;

            deliveriesDto.Add(deliveryDto);
        }

        return deliveriesDto;
    }
}
