using FluentValidation;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Invoice
{
    public class InvoiceLineDtoValidator : AbstractValidator<InvoiceLineDto>
    {
        public InvoiceLineDtoValidator()
        {
            RuleFor(t => t.Qty)
                .NotEmpty()
                .Must(t => t!.Value > 0)
                .WithName("Qte");
        }
    }
}
