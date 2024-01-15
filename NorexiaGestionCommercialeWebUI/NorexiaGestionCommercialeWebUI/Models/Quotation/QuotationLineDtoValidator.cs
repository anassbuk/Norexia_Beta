using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;
namespace NorexiaGestionCommercialeWebUI.Models;

    public class QuotationCommandLineDtoValidator : AbstractValidator<QuotationLineDto>
    {
    public QuotationCommandLineDtoValidator()
    {
            RuleFor(t => t.Qty)
                .NotEmpty()
                .Must(t => t!.Value > 0)
                .WithName("Qte");

            RuleFor(t => t.SellingPriceId)
                .NotEmpty()
                .WithName("Prix de vente");
        }
    }

