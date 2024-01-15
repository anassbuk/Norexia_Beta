using MediatR;
using Microsoft.EntityFrameworkCore;
using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProviderInvoiceEntities;

namespace Norexia.Core.Application.ProviderInvoices.Commands.CreateProviderInvoice;
public class CreateProviderInvoiceCommandHandler : IRequestHandler<CreateProviderInvoiceCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileServices _fileServices;
    public CreateProviderInvoiceCommandHandler(IFileServices fileServices, IApplicationDbContext context)
    {
        _context = context;
        _fileServices = fileServices;
    }

    public async Task<Guid> Handle(CreateProviderInvoiceCommand request, CancellationToken cancellationToken)
    {
        if (await _context.ProviderInvoices.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Provider Invoice with reference ({request.Reference}) already exist! ");

        if (!await _context.Providers.AnyAsync(c => c.Id == request.ProviderId, cancellationToken))
            throw new NotFoundException($"Provider with id ({request.ProviderId}) not found! ");

        if (request.ProviderInvoiceOrigin == ProviderInvoiceOrigin.PurchaseOrder)
            if (!await _context.PurchaseOrders.AnyAsync(c => c.Id == request.PurchaseOrderId, cancellationToken))
                throw new NotFoundException($"Purchase order with id ({request.PurchaseOrderId}) not found! ");


        ProviderInvoice invoice = new()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            ProviderId = request.ProviderId,
            PurchaseOrderId = request.PurchaseOrderId,
            CurrencyId = request.CurrencyId,
            ProviderInvoiceOrigin = request.ProviderInvoiceOrigin,
            Discount = request.Discount,
            PaymentTerms = request.PaymentTerms,
            EntryDate = request.EntryDate.ToUniversalTime(),
            DueDate = request.DueDate?.ToUniversalTime(),
            Status = request.Status,
            DirectCreationReason = request.DirectCreationReason,
            Note = request.Note,
        };

        invoice.ProviderInvoiceLines = await HandleInvoiceLines(request, cancellationToken);
        invoice.AttachedDigitalInvoices = await HandleAttachedDigitalInvoices(request, invoice.Id, cancellationToken);
        invoice.ProviderInvoicePayments = await HandleInvoicePayments(request, cancellationToken);

        var result = await _context.ProviderInvoices.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }

    public async Task<List<ProviderInvoiceLine>> HandleInvoiceLines(CreateProviderInvoiceCommand request, CancellationToken cancellationToken)
    {
        List<ProviderInvoiceLine> invoiceLines = new();
        foreach (var line in request.ProviderInvoiceLines!)
        {
            if (!await _context.Products.AnyAsync(c => c.Id == line.ProductId, cancellationToken))
                throw new NotFoundException($"Product with id ({line.ProductId}) not found! ");

            invoiceLines.Add(new ProviderInvoiceLine()
            {
                ProductId = line.ProductId,
                Price = line.Price,
                VAT = line.VAT,
                Qty = line.Qty,
                ExpectedQty = line.ExpectedQty,
            });
        }

        return invoiceLines;
    }

    public async Task<List<AttachedDigitalInvoice>?> HandleAttachedDigitalInvoices(CreateProviderInvoiceCommand request,Guid invoiceId, CancellationToken cancellationToken)
    {

        if (request.AttachedDigitalInvoices is null || request.AttachedDigitalInvoices.Count == 0)
            return null;

        var attachedDigitalInvoices = new List<AttachedDigitalInvoice>();
        foreach (var item in request.AttachedDigitalInvoices)
        {
            var path = await _fileServices.StoreAttachedDigitalInvoiceAsync(request.Reference!, item.File!.Name!, item.File!.Base64Data!, cancellationToken);

            attachedDigitalInvoices.Add(new ()
            {
                Id = Guid.NewGuid(),
                InvoiceId = invoiceId,
                Label = item.Label,
                Path = path,
            });
        }

        return attachedDigitalInvoices;
    }

    public async Task<List<ProviderInvoicePayment>> HandleInvoicePayments(CreateProviderInvoiceCommand request, CancellationToken cancellationToken)
    {
        List<ProviderInvoicePayment> InvoicePayments = new();
        foreach (var payment in request.ProviderInvoicePayments!)
        {
            if (await _context.InvoicePayments.AnyAsync(o => o.Reference == payment.Reference))
                throw new NotFoundException($"Payment with reference ({payment.Reference}) already exist! ");

            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == payment.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({payment.PaymentMeanId}) not found! ");

            InvoicePayments.Add(new ProviderInvoicePayment()
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

        return InvoicePayments;
    }
}
