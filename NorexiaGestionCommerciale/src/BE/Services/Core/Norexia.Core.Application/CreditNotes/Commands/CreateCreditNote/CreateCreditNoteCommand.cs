using MediatR;
using Norexia.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Commands.CreateCreditNote
{
    public class CreateCreditNoteCommand :IRequest<Guid>
    {
        public string? CreditNumber { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? CustomerId { get; set; }
        public string? Responsable { get; set; }
        public string? Raison { get; set; }
        public string? Note { get; set; }
        public double CreditAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public ICollection<CreditNoteLineCommand>? CreditNoteLines { get; set; }
        public CreditOrigin CreditOrigin { get; set; }
        public CreditAction CreditAction { get; set; }
    }
}
