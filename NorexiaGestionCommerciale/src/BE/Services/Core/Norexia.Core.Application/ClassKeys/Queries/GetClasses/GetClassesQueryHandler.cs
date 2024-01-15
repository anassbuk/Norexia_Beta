using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.ClassKeys.Queries.GetClasses;

public record GetClassesQuery : IRequest<IEnumerable<ClassDto>>;
public class GetClassesQueryHandler : IRequestHandler<GetClassesQuery, IEnumerable<ClassDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetClassesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClassDto>> Handle(GetClassesQuery request, CancellationToken cancellationToken)
    {
        return await _context.ClassKeys
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .Include(c => c.Values!.Where(v => v.IsDeleted == false))
                        .ProjectTo<ClassDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
