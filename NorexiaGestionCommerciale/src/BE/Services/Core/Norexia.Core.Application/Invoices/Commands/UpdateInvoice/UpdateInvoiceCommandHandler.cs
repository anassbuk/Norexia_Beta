using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Invoices.Commands.UpdateInvoice;
public class UpdateInvoiceCommandHandler : IRequestHandler<UpdateInvoiceCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public UpdateInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _context.Invoices
                                .Include(s => s.InvoiceLines)
                                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (invoice == null)
            throw new NotFoundException($"Invoice with id ({request.Id}) not found!");

        invoice.CustomerId = request.CustomerId;
        invoice.SaleOrderId = request.SaleOrderId;
        invoice.DeliveryRef = request.DeliveryRef;
        invoice.CurrencyId = request.CurrencyId;
        invoice.InvoiceOrigin = request.InvoiceOrigin;
        invoice.EntryDate = request.EntryDate!.ToUniversalTime();
        invoice.DueDate = request.DueDate?.ToUniversalTime();
        invoice.DeliveryStartDate = request.DeliveryStartDate?.ToUniversalTime();
        invoice.DeliveryEndDate = request.DeliveryEndDate?.ToUniversalTime();
        invoice.Discount = request.Discount;
        invoice.InvoiceType = request.InvoiceType;
        invoice.PaymentTerms = request.PaymentTerms!;
        invoice.DirectCreationReason = request.DirectCreationReason;
        invoice.Status = request.Status;
        invoice.Note = request.Note;

        await HandleInvoiceLines(request, invoice, cancellationToken);
        await HandleInvoicePayments(request, invoice, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return invoice.Id;
    }

    public async Task HandleInvoiceLines(UpdateInvoiceCommand request, Invoice invoice, CancellationToken cancellationToken)
    {
        var linesToRemove = invoice.InvoiceLines!
            .Where(l => !request.InvoiceLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            invoice.InvoiceLines!.Remove(line);

        foreach (var line in request.InvoiceLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(line.SellingPriceId, cancellationToken);

            if (sellingPrice is null)
                throw new NotFoundException($"Selling Price with id ({line.SellingPriceId}) not found! ");

            if (invoice.InvoiceLines!.Any(l => l.Id == line.Id))
            {
                var item = invoice.InvoiceLines!.First(l => l.Id == line.Id);
                item.ProductId = sellingPrice.ProductId;
                item.SellingPriceId = line.SellingPriceId;
                item.DeliveryRef = line.DeliveryRef;
                item.Price = sellingPrice.Price;
                item.VAT = sellingPrice.VAT;
                item.Discount = sellingPrice.Discount;
                item.Qty = line.Qty;
                item.ExpectedQty = line.ExpectedQty;
            }
            else
            {
                invoice.InvoiceLines!.Add(new InvoiceLine()
                {
                    ProductId = sellingPrice.ProductId,
                    SellingPriceId = line.SellingPriceId,
                    Price = sellingPrice.Price,
                    DeliveryRef = line.DeliveryRef,
                    VAT = sellingPrice.VAT,
                    Discount = sellingPrice.Discount,
                    Qty = line.Qty,
                    ExpectedQty = line.ExpectedQty
                });
            }
        }
    }

    public async Task HandleInvoicePayments(UpdateInvoiceCommand request, Invoice invoice, CancellationToken cancellationToken)
    {
        var linesToRemove = invoice.InvoicePayments!
            .Where(l => !request.InvoicePayments!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            invoice.InvoicePayments!.Remove(line);

        foreach (var payment in request.InvoicePayments!)
        {
            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == payment.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({payment.PaymentMeanId}) not found! ");

            if (invoice.InvoicePayments!.Any(l => l.Id == payment.Id))
            {
                var invoicePayment = invoice.InvoicePayments!.First(l => l.Id == payment.Id);

                invoicePayment.PaymentMeanId = payment.PaymentMeanId;
                invoicePayment.EntryDate = payment.EntryDate.ToUniversalTime();
                invoicePayment.DueDate = payment.DueDate?.ToUniversalTime();
                invoicePayment.OperationDate = payment.OperationDate?.ToUniversalTime();
                invoicePayment.Status = payment.Status;
                invoicePayment.Note = request.Note;
                invoicePayment.AmountToBePaid = payment.AmountToBePaid;
                invoicePayment.AmountPaid = payment.AmountPaid;
            }
            else
            {
                if (await _context.InvoicePayments.AnyAsync(o => o.Reference == payment.Reference))
                    throw new NotFoundException($"Payment with reference ({payment.Reference}) already exist! ");

                invoice.InvoicePayments!.Add(new InvoicePayment()
                {
                    Id = Guid.NewGuid(),
                    Reference = payment.Reference,
                    PaymentMeanId = payment.PaymentMeanId,
                    EntryDate = payment.EntryDate.ToUniversalTime(),
                    DueDate = payment.DueDate?.ToUniversalTime(),
                    OperationDate = payment.OperationDate?.ToUniversalTime(),
                    Status = payment.Status,
                    Note = request.Note,
                    AmountToBePaid = payment.AmountToBePaid,
                    AmountPaid = payment.AmountPaid,
                });
            }
        }
    }
}
