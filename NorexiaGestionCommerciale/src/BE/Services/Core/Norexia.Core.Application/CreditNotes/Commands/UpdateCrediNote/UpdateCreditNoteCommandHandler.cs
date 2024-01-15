using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.CreditNoteEntities;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.ProductEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Commands.UpdateCrediNote;
public class UpdateCreditNoteCommandHandler : IRequestHandler<UpdateCreditNoteCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdateCreditNoteCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(UpdateCreditNoteCommand request, CancellationToken cancellationToken)
    {
        var creditNote = await _context.CreditNotes
                                .Include(c => c.CreditNoteLines)
                               .SingleOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (creditNote == null)
            throw new NotFoundException($"CreditNote with id ({request.Id}) not found! ");

        creditNote.CustomerId = request.CustomerId;
        creditNote.InvoiceId = request.InvoiceId;
        creditNote.CreditNumber = request.CreditNumber;
        creditNote.CreditNoteDate = request.CreditNoteDate!.Value.ToUniversalTime();
        creditNote.Responsable = request.Responsable;
        creditNote.Raison = request.Raison;
        creditNote.Note = request.Note;
        creditNote.CreditAmount = request.CreditAmount;
        creditNote.DueDate = request.DueDate?.ToUniversalTime();
        creditNote.CreditOrigin = request.CreditOrigin;
        creditNote.CreditAction = request.CreditAction;

        await HandleCreditNoteLines(request, creditNote, cancellationToken);
       

        await _context.SaveChangesAsync(cancellationToken);
        return creditNote.Id;


    }

    private async Task HandleCreditNoteLines(UpdateCreditNoteCommand request, CreditNote creditNote, CancellationToken cancellationToken)
    {
        var linesToRemove = creditNote.CreditNoteLines!
            .Where(l => !request.CreditNoteLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            creditNote.CreditNoteLines!.Remove(line);

        foreach (var line in request.CreditNoteLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(line.SellingPriceId, cancellationToken);
            if (sellingPrice is null)
                throw new NotFoundException($"SellingPrice with id ({line.SellingPriceId}) not found! ");

            if (creditNote.CreditNoteLines!.Any(l => l.Id == line.Id))
            {
                var item = creditNote.CreditNoteLines!.Single(l => l.Id == line.Id);
                item.ProductId = sellingPrice.ProductId;
                item.SellingPriceId = line.SellingPriceId;
                item.DeliveryRef = line.DeliveryRef;
                item.Price = sellingPrice.Price;
                item.VAT = sellingPrice.VAT;
                item.Discount = sellingPrice.Discount;
                item.Qty = line.Qty;
                
            }
            else
            {
                creditNote.CreditNoteLines!.Add(new CreditNoteLine()
                {
                    ProductId = sellingPrice.ProductId,
                    SellingPriceId = line.SellingPriceId,
                    Price = sellingPrice.Price,
                    DeliveryRef = line.DeliveryRef,
                    VAT = sellingPrice.VAT,
                    Discount = sellingPrice.Discount,
                    Qty = line.Qty,
                });
            }
        }

    }
   
}
        
    
