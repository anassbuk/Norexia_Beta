using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.CreditNote
{
    public class CreditNoteLineDtoValidator : AbstractValidator<CreditNoteLineDto>
    {
        public CreditNoteLineDtoValidator()
        {
            RuleFor(c => c.SellingPriceId)
                .NotEmpty();

            RuleFor(c => c.Reference)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.DeliveryRef)
                .MaximumLength(200);

            RuleFor(c => c.ProductId)
                .NotEmpty();


            RuleFor(c => c.Vat)
                .InclusiveBetween(0, 100);

            RuleFor(c => c.Discount)
                .InclusiveBetween(0, 100);

            RuleFor(c => c.Qty)
                .GreaterThan(0);

            RuleFor(c => c.ExpectedQty)
                .GreaterThanOrEqualTo(0);
        }
    }
}

