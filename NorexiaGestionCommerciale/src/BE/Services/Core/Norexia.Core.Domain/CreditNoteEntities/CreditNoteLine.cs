using Norexia.Core.Domain.Common;
using Norexia.Core.Domain.InvoiceEntities;
using Norexia.Core.Domain.ProductEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Domain.CreditNoteEntities
{
    public class CreditNoteLine : BaseAuditableEntity
    {
        public Guid? SellingPriceId { get; set; }
        public virtual SellingPrice? SellingPrice { get; set; }
        public string? DeliveryRef { get; set; }
        public Guid CreditNoteId { get; set; }
        public virtual CreditNote? CreditNote { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public double? Price { get; set; }
        public int? VAT { get; set; }
        public int? Discount { get; set; }
        public int? Qty { get; set; }
        public int? ExpectedQty { get; set; }
    }
}
