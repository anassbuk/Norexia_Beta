using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProviderInvoiceEntities;

namespace Norexia.Core.Application.ProviderInvoices.Commands.UpdateProviderInvoice;
public class UpdateProviderInvoiceCommandHandler : IRequestHandler<UpdateProviderInvoiceCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileServices _fileServices;
    public UpdateProviderInvoiceCommandHandler(IApplicationDbContext context, IFileServices fileServices)
    {
        _context = context;
        _fileServices = fileServices;
    }

    public async Task<Guid> Handle(UpdateProviderInvoiceCommand request, CancellationToken cancellationToken)
    {
        var invoice = await _context.ProviderInvoices
                                .Include(s => s.ProviderInvoiceLines)
                                .Include(s => s.AttachedDigitalInvoices)
                                .SingleOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (invoice == null)
            throw new NotFoundException($"Invoice with id ({request.Id}) not found!");
        if (!await _context.Providers.AnyAsync(c => c.Id == request.ProviderId, cancellationToken))
            throw new NotFoundException($"Provider with id ({request.ProviderId}) not found! ");

        if (request.ProviderInvoiceOrigin == ProviderInvoiceOrigin.PurchaseOrder)
            if (!await _context.PurchaseOrders.AnyAsync(c => c.Id == request.PurchaseOrderId, cancellationToken))
                throw new NotFoundException($"Purchase order with id ({request.PurchaseOrderId}) not found! ");

        invoice.Reference = request.Reference;
        invoice.ProviderId = request.ProviderId;
        invoice.PurchaseOrderId = request.PurchaseOrderId;
        invoice.CurrencyId = request.CurrencyId;
        invoice.ProviderInvoiceOrigin = request.ProviderInvoiceOrigin;
        invoice.Discount = request.Discount;
        invoice.PaymentTerms = request.PaymentTerms;
        invoice.EntryDate = request.EntryDate.ToUniversalTime();
        invoice.DueDate = request.DueDate?.ToUniversalTime();
        invoice.Status = request.Status;
        invoice.DirectCreationReason = request.DirectCreationReason;
        invoice.Note = request.Note;

        await HandleInvoiceLines(request, invoice, cancellationToken);
        await HandleAttachedDigitalInvoices(request, invoice, cancellationToken);
        await HandleInvoicePayments(request, invoice, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return invoice.Id;
    }


    public async Task HandleInvoiceLines(UpdateProviderInvoiceCommand request, ProviderInvoice invoice, CancellationToken cancellationToken)
    {
        var linesToRemove = invoice.ProviderInvoiceLines!
            .Where(l => !request.ProviderInvoiceLines!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            invoice.ProviderInvoiceLines!.Remove(line);

        foreach (var line in request.ProviderInvoiceLines!)
        {
            if (!await _context.Products.AnyAsync(c => c.Id == line.ProductId, cancellationToken))
                throw new NotFoundException($"Product with id ({line.ProductId}) not found! ");

            if (invoice.ProviderInvoiceLines!.Any(l => l.Id == line.Id))
            {
                var item = invoice.ProviderInvoiceLines!.First(l => l.Id == line.Id);

                item.ProductId = line.ProductId;
                item.Price = line.Price;
                item.VAT = line.VAT;
                item.Qty = line.Qty;
                item.ExpectedQty = line.ExpectedQty;
            }
            else
            {
                invoice.ProviderInvoiceLines!.Add(new ProviderInvoiceLine()
                {
                    ProductId = line.ProductId,
                    Price = line.Price,
                    VAT = line.VAT,
                    Qty = line.Qty,
                    ExpectedQty = line.ExpectedQty,
                });
            }
        }
    }

    public async Task HandleAttachedDigitalInvoices(UpdateProviderInvoiceCommand request, ProviderInvoice invoice, CancellationToken cancellationToken)
    {
        var invoicesToRemove = invoice.AttachedDigitalInvoices!
            .Where(l => !request.AttachedDigitalInvoices!.Any(rl => rl.Id == l.Id));

        foreach (var item in invoicesToRemove.ToList())
        {
            invoice.AttachedDigitalInvoices!.Remove(item);
            if (File.Exists(item.Path))
                File.Delete(item.Path!);
        }

        foreach (var item in request.AttachedDigitalInvoices!)
        {
            if (!invoice.AttachedDigitalInvoices!.Any(l => l.Id == item.Id))
            {
                var path = await _fileServices.StoreAttachedDigitalInvoiceAsync(request.Reference!, item.File!.Name!, item.File!.Base64Data!, cancellationToken);

                invoice.AttachedDigitalInvoices!.Add(new()
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoice.Id,
                    Label = item.Label,
                    Path = path,
                });
            }
        }
    }

    public async Task HandleInvoicePayments(UpdateProviderInvoiceCommand request, ProviderInvoice invoice, CancellationToken cancellationToken)
    {
        var linesToRemove = invoice.ProviderInvoicePayments!
            .Where(l => !request.ProviderInvoicePayments!.Any(rl => rl.Id == l.Id));

        foreach (var line in linesToRemove.ToList())
            invoice.ProviderInvoicePayments!.Remove(line);

        foreach (var payment in request.ProviderInvoicePayments!)
        {
            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == payment.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({payment.PaymentMeanId}) not found! ");

            if (invoice.ProviderInvoicePayments!.Any(l => l.Id == payment.Id))
            {
                var invoicePayment = invoice.ProviderInvoicePayments!.First(l => l.Id == payment.Id);

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
                if (await _context.ProviderInvoicePayments.AnyAsync(o => o.Reference == payment.Reference))
                    throw new NotFoundException($"Payment with reference ({payment.Reference}) already exist! ");

                invoice.ProviderInvoicePayments!.Add(new ProviderInvoicePayment()
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
