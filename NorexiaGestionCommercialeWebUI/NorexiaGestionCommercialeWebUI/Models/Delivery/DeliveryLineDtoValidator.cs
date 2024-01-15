using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Delivery;
public class DeliveryLineDtoValidator : AbstractValidator<DeliveryLineDto>
{
    public DeliveryLineDtoValidator()
    {
        RuleFor(t => t.Qty)
            .NotEmpty()
            .Must(t => t!.Value > 0)
            .WithName("Qte");
    }
}
