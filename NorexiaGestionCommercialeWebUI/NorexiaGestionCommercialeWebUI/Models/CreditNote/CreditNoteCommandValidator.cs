using FluentValidation;

namespace NorexiaGestionCommercialeWebUI.Models.CreditNote
{
    public class CreditNoteCommandValidator : AbstractValidator<CreditNoteCommand>
    {
        public CreditNoteCommandValidator()
        {
            RuleFor(c => c.CreditNumber)
                .NotEmpty()
                .MaximumLength(50);

            //RuleFor(c => c.CreditNoteDate)
            //    .NotEmpty();

            RuleFor(c => c.CustomerId)
                .NotEmpty();

            RuleFor(c => c.InvoiceId)
                .NotEmpty();

            RuleFor(c => c.CreditOrigin)
                .IsInEnum();


            RuleFor(c => c.Responsable)
                    .NotEmpty()
                   .MaximumLength(200);

            RuleFor(c => c.Raison)
                 .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Note)
                .MaximumLength(200);

            RuleFor(c => c.CreditAction)
                .NotEmpty();



            //RuleFor(c => c.CreditNoteLines)
            //    .NotNull()
            //    .Must(l => l.Count > 0)
            //    .WithMessage("The number of lines is insufficient");

        }
    }
}
