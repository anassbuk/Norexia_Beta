using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.InvoiceEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Queries.GetCreditNotes
{
    public record GetCreditNotesQuery : IRequest<IEnumerable<CreditNoteDto>>;
    public class GetCreditNotesQueryHandler : IRequestHandler<GetCreditNotesQuery, IEnumerable<CreditNoteDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetCreditNotesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CreditNoteDto>> Handle(GetCreditNotesQuery request, CancellationToken cancellationToken)
        {
        

            var CreditNotes = await _context.CreditNotes
                                    .AsNoTracking()
                                    .Where(c => c.IsDeleted == false)
                                    .Include(c => c.Customer)
                                    .ThenInclude(c => c!.CustomerAddresses)
                                    .Include(c => c.Invoice)
                                    .Include(c => c.CreditNoteLines)
                                    .ToListAsync(cancellationToken);

                                  

            var creditNotesDto = new List<CreditNoteDto>();
      

            foreach (var creditNote in CreditNotes)
            {
               var creditNoteDto = _mapper.Map<CreditNoteDto>(creditNote);
                if (creditNote.CreditOrigin == CreditOrigin.Invoice)
                    creditNoteDto.InvoiceRef = creditNote.Invoice!.Reference;

            
                creditNoteDto.CustomerRef = $"{creditNote.Customer!.Reference} - {(creditNote.Customer!.ClientType == ClientType.Particular ? $"{creditNote.Customer!.FirstName}, {creditNote.Customer!.LastName}" : creditNote.Customer!.SocialReason)}";
                //calculate amount credit from credit note lines
                var AmountCredit = creditNote.CreditNoteLines!.Select(line => line.Qty * (line.Price - (line.Price * (((double?)line.Discount ?? 0) / 100)))).Sum();
                var taxPrice = creditNote.CreditNoteLines!.Select(l => l.Qty * l.Price * (((double?)l.VAT ?? 0) / 100)).Sum();

                creditNoteDto.AmountCredit = AmountCredit + taxPrice;
              
              
                

           
                creditNotesDto.Add(creditNoteDto);
              
            }
            return creditNotesDto;
           

        }
    }

}

