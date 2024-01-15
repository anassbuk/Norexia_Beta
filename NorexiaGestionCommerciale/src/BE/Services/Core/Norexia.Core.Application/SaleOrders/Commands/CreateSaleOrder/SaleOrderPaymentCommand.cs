using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.SaleOrders.Commands.CreateSaleOrder;
public class SaleOrderPaymentCommand
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? PaymentMeanId { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? OperationDate { get; set; }
    public string? Status { get; set; }
    public double? AmountToBePaid { get; set; }
    public double? AmountPaid { get; set; }
}
