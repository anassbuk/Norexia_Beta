using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Commands.UpdateCrediNote;
public class UpdateCreditNoteCommandValidator : AbstractValidator<UpdateCreditNoteCommand>
{
    public UpdateCreditNoteCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.CreditNumber)
            .NotNull()
            .NotEmpty();

        RuleFor(t => t.CreditNoteLines)
           .NotNull()
           .Must(l => l!.Count > 0)
           .WithMessage("The number of lines is insufficient");

    }
}
