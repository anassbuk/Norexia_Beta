using FluentValidation;

using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.StockEntries.Commands.UpdateStockEntry;
public class UpdateStockEntryCommandValidator : AbstractValidator<UpdateStockEntryCommand>
{
    public UpdateStockEntryCommandValidator()
    {

        RuleFor(t => t.Reference)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.StockEntryOrigin)
            .NotNull();

        RuleFor(t => t.EntryDate)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.Type)
            .NotNull();

        When(t => t.StockEntryOrigin == StockEntryOrigin.PurchaseOrder, () =>
        {
            RuleFor(t => t.PurchaseOrderId)
                .NotNull()
                .NotEmpty();
        });

        When(t => t.StockEntryOrigin == StockEntryOrigin.DirectCreation ||
                    t.StockEntryOrigin == StockEntryOrigin.PurchaseOrder, () =>
                    {
                        RuleFor(t => t.ProviderId)
                        .NotNull()
                        .NotEmpty();
                    });

        RuleFor(t => t.StockEntryLines)
        .NotNull()
        .Must(l => l!.Count > 0)
        .WithMessage("The number of lines is insufficient");
    }
}
