using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Application.Invoices.Queries.CreditNoteLineDto;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CreditNoteEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Queries.GetCreditNotes
{
    public class CreditNoteDto : IMapFrom<CreditNote>
    {
        public Guid Id { get; set; }
        public string? CreditNumber { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public Guid? InvoiceId { get; set; }
        public string? InvoiceRef { get; set; }
        public Guid? CustomerId { get; set; }
        public string? CustomerRef { get; set; }
        public string? Responsable { get; set; }
        public string? Raison { get; set; }
        public string? Note { get; set; }
        public double? AmountCredit { get; set; }
        public CreditOrigin CreditOrigin { get; set; }
        public CreditAction CreditAction { get; set; }
        public DateTime? DueDate { get; set; }

        public ICollection<CreditNoteLineDto>? CreditNoteLines { get; set; }
    }
}
