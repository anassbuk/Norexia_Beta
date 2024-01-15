using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CreditNoteEntities;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Commands.CreateCreditNote
{
    public class CreateCreditNoteCommandHandler : IRequestHandler<CreateCreditNoteCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        public CreateCreditNoteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> Handle(CreateCreditNoteCommand request, CancellationToken cancellationToken)
        {
            
            if (await _context.CreditNotes.AnyAsync(o => o.CreditNumber == request.CreditNumber))
                throw new NotFoundException($"Credit note with reference ({request.CreditNumber}) already exist! ");    

            if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken))
                throw new NotFoundException($"Customer with id ({request.CustomerId}) not found! ");

            if (request.CreditOrigin == CreditOrigin.Invoice)
                if (!await _context.Invoices.AnyAsync(c => c.Id == request.InvoiceId, cancellationToken))
                    throw new NotFoundException($"Invoice with id ({request.InvoiceId}) not found! ");

            CreditNote creditNote = new()
            {
                Id = Guid.NewGuid(),
                CreditNumber = request.CreditNumber,
                CreditNoteDate = request.CreditNoteDate?.ToUniversalTime(),
                InvoiceId = request.InvoiceId,
                CustomerId = request.CustomerId,
                Responsable = request.Responsable,
                Raison = request.Raison,
                Note = request.Note,
                CreditAmount = request.CreditAmount,
                DueDate = request.DueDate?.ToUniversalTime(),
                CreditOrigin = request.CreditOrigin,
                CreditAction = request.CreditAction,
            };  

            creditNote.CreditNoteLines =HandleCreditNoteLines(request, cancellationToken);

            var result = await _context.CreditNotes.AddAsync(creditNote, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity.Id;
            

           
        }

        public List<CreditNoteLine> HandleCreditNoteLines(CreateCreditNoteCommand request, CancellationToken cancellationToken)
        {
            List<CreditNoteLine> creditNoteLines = new();

            foreach (var line in request.CreditNoteLines!)
            {
                SellingPrice? sellingPrice = _context.SellingPrices.Where(x => x.Id == line.SellingPriceId).FirstOrDefault();
                if (sellingPrice == null)
                    throw new NotFoundException($"Selling price with id ({line.SellingPriceId}) not found! ");

                creditNoteLines.Add(new CreditNoteLine()
                {
                    ProductId = sellingPrice.ProductId,
                    SellingPriceId = line.SellingPriceId,
                    Price = sellingPrice.Price,
                    VAT = sellingPrice.VAT,
                    DeliveryRef = line.DeliveryRef,
                    Discount = sellingPrice.Discount,
                    Qty = line.Qty,
                    ExpectedQty = line.ExpectedQty,
                });
            }
            return creditNoteLines;
        }

    }
}
