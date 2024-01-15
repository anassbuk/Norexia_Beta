using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.StockEntry;
public class StockEntryLineDtoValidator : AbstractValidator<StockEntryLineDto>
{
    public StockEntryLineDtoValidator()
    {
        RuleFor(t => t.Qty)
            .NotEmpty()
            .Must(t => t!.Value > 0)
            .WithName("Qte");
    }
}
