using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.ClassKeys.Queries.GetClasses;
using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.ClassValues.Queries;

public record GetClassValuesQuery(Guid ClassId) : IRequest<IEnumerable<ClassValueDto>>;
public class GetClassValuesQueryHandler : IRequestHandler<GetClassValuesQuery, IEnumerable<ClassValueDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetClassValuesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClassValueDto>> Handle(GetClassValuesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ClassValues
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false && c.ProductClassKeyId == request.ClassId)
                        .ProjectTo<ClassValueDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
