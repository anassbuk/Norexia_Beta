using FluentValidation;
using MediatR;

using Norexia.Core.Domain.Common.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentCommandValidator()
        {
            RuleFor(t => t.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(t => t.Reference)
                .NotNull()
                .NotEmpty();

            RuleFor(t => t.EntryDate)
                .NotNull()
                .NotEmpty();

            RuleFor(t => t.PaymentOrigin)
                .NotNull();

            RuleFor(t => t.DueDate)
                .NotNull()
                .NotEmpty();

            RuleFor(t => t.PaymentMeanId)
                .NotNull()
                .NotEmpty();


            When(t => t.PaymentOrigin == PaymentOrigin.Invoice, () =>
            {
                RuleFor(t => t.InvoiceId)
                    .NotNull()
                    .NotEmpty();
            });

            When(t => t.PaymentOrigin == PaymentOrigin.SaleOrder, () =>
            {
                RuleFor(t => t.SaleOrderId)
                    .NotNull()
                    .NotEmpty();
            });
        }
    }
}
