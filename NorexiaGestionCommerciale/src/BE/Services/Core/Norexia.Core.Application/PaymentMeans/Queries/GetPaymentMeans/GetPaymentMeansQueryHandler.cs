using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.PaymentMeans.Queries.GetPaymentMeans;
public record class GetPaymentMeansQuery : IRequest<IEnumerable<PaymentMeanDto>>;
public class GetPaymentMeansQueryHandler : IRequestHandler<GetPaymentMeansQuery, IEnumerable<PaymentMeanDto>>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetPaymentMeansQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PaymentMeanDto>> Handle(GetPaymentMeansQuery request, CancellationToken cancellationToken)
    {
        return await _context.PaymentMeans
                        .AsNoTracking()
                        .Where(c => c.IsDeleted == false)
                        .ProjectTo<PaymentMeanDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}
