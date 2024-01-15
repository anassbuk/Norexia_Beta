using Microsoft.AspNetCore.Components;
using NorexiaGestionCommercialeWebUI.Models.Payment;

namespace NorexiaGestionCommercialeWebUI.Components.Payment;
public partial class PaymentNoteComponent
{
    [Parameter]
    public PaymentCommand? PaymentCommand { get; set; }
    [Parameter]
    public EventCallback<PaymentCommand> PaymentCommandChanged { get; set; }
}
