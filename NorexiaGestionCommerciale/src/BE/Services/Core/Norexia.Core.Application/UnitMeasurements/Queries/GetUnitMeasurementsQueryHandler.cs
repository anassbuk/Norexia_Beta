using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Application.UnitTypes.Queries.GetUnits;

namespace Norexia.Core.Application.UnitMeasurements.Queries;

public record GetUnitMeasurementsQuery(Guid UnitId) : IRequest<IEnumerable<UnitMeasurementDto>>;
internal class GetUnitMeasurementsQueryHandler : IRequestHandler<GetUnitMeasurementsQuery, IEnumerable<UnitMeasurementDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetUnitMeasurementsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UnitMeasurementDto>> Handle(GetUnitMeasurementsQuery request, CancellationToken cancellationToken)
    {
        return await _context.UnitMeasurements
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false && c.UnitTypeId == request.UnitId)
                        .ProjectTo<UnitMeasurementDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
