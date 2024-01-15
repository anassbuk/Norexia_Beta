using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Components.Payment;
public partial class AssociatedPaymentsComponent
{
    [Parameter]
    public List<PaymentDto>? AssociatedPayments { get; set; }
}
