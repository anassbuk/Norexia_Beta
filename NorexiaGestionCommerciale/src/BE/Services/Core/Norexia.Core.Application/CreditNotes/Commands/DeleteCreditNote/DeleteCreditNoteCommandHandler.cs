using MediatR;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Commands.DeleteCreditNote;
public record DeleteCreditNoteCommand(IEnumerable<Guid> Ids) : IRequest;
public class DeleteCreditNoteCommandHandler : IRequestHandler<DeleteCreditNoteCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCreditNoteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(DeleteCreditNoteCommand request, CancellationToken cancellationToken)
    {
        foreach(var id in request.Ids)
        {
            var creditNote = _context.CreditNotes.SingleOrDefault(t => t.Id == id);
            if (creditNote == null)
            {
                throw new NotFoundException($"CreditNote with id ({id}) not found!");
            }
            creditNote.IsDeleted = true;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }
}
