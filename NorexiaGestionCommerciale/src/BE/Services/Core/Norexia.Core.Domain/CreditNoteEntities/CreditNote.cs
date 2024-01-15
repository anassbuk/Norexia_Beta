using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.CustomerEntities;
using Norexia.Core.Domain.InvoiceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Domain.CreditNoteEntities
{
    public class CreditNote : BaseAuditableEntity
    {
        
        public string? CreditNumber { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? CustomerId { get; set; }
        public virtual Invoice? Invoice { get; set; }        
        public virtual Customer? Customer { get; set; }
        public CreditOrigin CreditOrigin { get; set; }
        public CreditAction CreditAction { get; set; }
        public string? Responsable { get; set; }    
        public string? Raison { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Note { get; set; }
        public double CreditAmount { get; set; }
        public virtual ICollection<CreditNoteLine>? CreditNoteLines { get; set; }




    }
}
