using Norexia.Core.Application.Customers.Queries.GetCustomer;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.SaleOrderEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Norexia.Core.Application.SaleOrders.Queries.GetQuotationAsSaleOrder;
public class QuotationAsSaleOrderDto
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public CustomerAddressDto? BillingCustomerAddress { get; set; }
    public CustomerAddressDto? DeliveryCustomerAddress { get; set; }
    public float? Discount { get; set; }
    public OwnedPaymentTerms? PaymentTerms { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }
    public ICollection<SaleOrderLineDto>? SaleOrderLines { get; set; }
}
