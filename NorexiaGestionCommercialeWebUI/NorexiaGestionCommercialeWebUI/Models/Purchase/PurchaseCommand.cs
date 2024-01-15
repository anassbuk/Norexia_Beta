using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Purchase;
public class PurchaseCommand
{
    public Guid Id { get; set; }
    public Guid? ProviderId { get; set; }
    public string? Reference { get; set; }
    public DateTimeOffset? OrderDate { get; set; } = DateTime.Now;
    public string? Note { get; set; }
    public ICollection<PurchaseOrderLineDto>? PurchaseOrderLines { get; set; }
}
