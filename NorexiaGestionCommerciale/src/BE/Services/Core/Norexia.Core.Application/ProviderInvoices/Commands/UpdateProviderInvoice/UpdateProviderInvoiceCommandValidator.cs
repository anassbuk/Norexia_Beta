using FluentValidation;

namespace Norexia.Core.Application.ProviderInvoices.Commands.UpdateProviderInvoice;
public class UpdateProviderInvoiceCommandValidator : AbstractValidator<UpdateProviderInvoiceCommand>
{
    public UpdateProviderInvoiceCommandValidator()
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

        RuleFor(t => t.ProviderId)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.ProviderInvoiceOrigin)
            .NotNull();

        When(t => t.ProviderInvoiceOrigin == Domain.Common.Enums.ProviderInvoiceOrigin.PurchaseOrder, () =>
        {
            RuleFor(t => t.PurchaseOrderId)
            .NotNull()
            .NotEmpty();
        });

        RuleFor(t => t.ProviderInvoiceLines)
            .NotNull()
            .Must(l => l!.Count > 0)
            .WithMessage("The number of lines is insufficient");
    }
}
