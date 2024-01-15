using MediatR;

using Microsoft.EntityFrameworkCore;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.Exceptions;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.PaymentEntities;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Invoices.Commands.CreateInvoice;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateInvoiceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Invoices.AnyAsync(o => o.Reference == request.Reference))
            throw new NotFoundException($"Invoice with reference ({request.Reference}) already exist! ");

        if (!await _context.Customers.AnyAsync(c => c.Id == request.CustomerId, cancellationToken))
            throw new NotFoundException($"Customer with id ({request.CustomerId}) not found! ");

        if (request.InvoiceOrigin == InvoiceOrigin.DeliveryMulti || request.InvoiceOrigin == InvoiceOrigin.SalesOrder)
            if (!await _context.SaleOrders.AnyAsync(c => c.Id == request.SaleOrderId, cancellationToken))
                throw new NotFoundException($"Sale order with id ({request.SaleOrderId}) not found! ");

        Invoice invoice = new()
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            CustomerId = request.CustomerId,
            SaleOrderId = request.SaleOrderId,
            DeliveryRef = request.DeliveryRef,
            CurrencyId = request.CurrencyId,
            InvoiceOrigin = request.InvoiceOrigin,
            Discount = request.Discount,
            InvoiceType = request.InvoiceType,
            PaymentTerms = request.PaymentTerms!,
            EntryDate = request.EntryDate.ToUniversalTime(),
            DueDate = request.DueDate?.ToUniversalTime(),
            DeliveryStartDate = request.DeliveryStartDate?.ToUniversalTime(),
            DeliveryEndDate = request.DeliveryEndDate?.ToUniversalTime(),
            Status = request.Status,
            DirectCreationReason = request.DirectCreationReason,
            Note = request.Note,
        };

        invoice.InvoiceLines = await HandleInvoiceLines(request, cancellationToken);
        invoice.InvoicePayments = await HandleInvoicePayments(request, cancellationToken);

        var result = await _context.Invoices.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }


    public async Task<List<InvoiceLine>> HandleInvoiceLines(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        List<InvoiceLine> invoiceLines = new();
        foreach (var line in request.InvoiceLines!)
        {
            SellingPrice? sellingPrice = await _context.SellingPrices.FindAsync(line.SellingPriceId, cancellationToken);

            if (sellingPrice is null)
                throw new NotFoundException($"Selling Price with id ({line.SellingPriceId}) not found! ");

            invoiceLines.Add(new InvoiceLine()
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

        return invoiceLines;
    }


    public async Task<List<InvoicePayment>> HandleInvoicePayments(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        List<InvoicePayment> InvoicePayments = new();
        foreach (var payment in request.InvoicePayments!)
        {
            if (await _context.InvoicePayments.AnyAsync(o => o.Reference == payment.Reference))
                throw new NotFoundException($"Payment with reference ({payment.Reference}) already exist! ");

            if (!await _context.PaymentMeans.AnyAsync(c => c.Id == payment.PaymentMeanId, cancellationToken))
                throw new NotFoundException($"Payment mean with id ({payment.PaymentMeanId}) not found! ");

            InvoicePayments.Add(new InvoicePayment()
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
