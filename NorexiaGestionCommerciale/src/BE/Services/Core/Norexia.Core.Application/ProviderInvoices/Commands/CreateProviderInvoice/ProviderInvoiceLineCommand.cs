using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.ProviderInvoices.Commands.CreateProviderInvoice;
public class ProviderInvoiceLineCommand
{
    public Guid? Id { get; set; }
    public Guid ProductId { get; set; }
    public double? Price { get; set; }
    public int? VAT { get; set; }
    public int? Qty { get; set; }
    public int? ExpectedQty { get; set; }
}
