using Newtonsoft.Json;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Delivery;
public class DeliveryCommand
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public Guid? CustomerId { get; set; }
    public string? CustomerRef { get; set; }
    public Guid? DelivererId { get; set; }
    public string? DelivererRef { get; set; }
    public Guid? SaleOrderId { get; set; }
    public string? SaleOrderRef { get; set; }
    public Guid? InvoiceId { get; set; }
    public string? InvoiceRef { get; set; }
    public DeliveryOrigin? DeliveryOrigin { get; set; }
    public DeliveryMode? DeliveryMode { get; set; }
    public DateTimeOffset? EntryDate { get; set; } = DateTime.Now;
    public DateTimeOffset? DeliveryDate { get; set; } = DateTime.Now;
    public DateTimeOffset? PlannedDate { get; set; } = DateTime.Now;
    public DateTimeOffset? DeliveryTime { get; set; } = DateTime.Now;
    public string? Status { get; set; }
    public string? Situation { get; set; }
    public StockRecordType? Type { get; set; }
    public string? Note { get; set; }
    public string? CreatedBy { get; set; }
    public ICollection<DeliveryLineDto>? DeliveryLines { get; set; }
}
