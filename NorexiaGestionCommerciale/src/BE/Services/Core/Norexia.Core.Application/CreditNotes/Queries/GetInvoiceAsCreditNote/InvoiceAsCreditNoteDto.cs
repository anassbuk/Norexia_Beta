using Norexia.Core.Application.Invoices.Queries.CreditNoteLineDto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.CreditNotes.Queries.GetInvoiceAsCreditNote;
public class InvoiceAsCreditNoteDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public virtual ICollection<CreditNoteLineDto>? CreditNoteLines { get; set; }
}
