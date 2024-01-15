using AutoMapper;
using AutoMapper.QueryableExtensions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;

namespace Norexia.Core.Application.PaymentTerms.Queries;

public record GetPaymentTermsQuery : IRequest<PaymentTermsDto>;
public class GetPaymentTermsQueryHandler : IRequestHandler<GetPaymentTermsQuery, PaymentTermsDto?>
{
    private IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetPaymentTermsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaymentTermsDto?> Handle(GetPaymentTermsQuery request, CancellationToken cancellationToken)
    {
        return await _context.PaymentTerms
                        .AsNoTracking()
                        .ProjectTo<PaymentTermsDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
    }
}
