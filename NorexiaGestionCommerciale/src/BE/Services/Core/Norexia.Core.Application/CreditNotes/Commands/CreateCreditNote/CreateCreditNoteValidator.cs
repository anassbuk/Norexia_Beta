using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Commands.CreateCreditNote
{
    public class CreateCreditNoteValidator:AbstractValidator<CreateCreditNoteCommand>
    {
        public CreateCreditNoteValidator()
        {
            //RuleFor(t => t.CreditNumber)
            //  .NotNull()
            //  .NotEmpty()
            //  .MaximumLength(50);

            RuleFor(t => t.CreditNoteDate)
                .NotNull();

            RuleFor(t => t.InvoiceId)
                .NotNull();

            RuleFor(t => t.CustomerId)
                .NotNull();

            RuleFor(t => t.Responsable)
                .NotNull()
                .MaximumLength(200);

            RuleFor(t => t.Raison)
                .MaximumLength(200);

            RuleFor(t => t.Note)
                .MaximumLength(200);

            RuleFor(t => t.CreditAmount)
                .GreaterThanOrEqualTo(0);

            RuleFor(t => t.CreditNoteLines)
                .NotNull()
                .Must(l => l!.Count > 0)
                .WithMessage("The number of lines is insufficient");


        }   

    }
}
