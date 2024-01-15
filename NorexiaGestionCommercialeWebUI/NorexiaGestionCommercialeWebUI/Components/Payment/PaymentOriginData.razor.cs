using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Payment;

namespace NorexiaGestionCommercialeWebUI.Components.Payment;
public partial class PaymentOriginData
{
    [Parameter]
    public PaymentCommand? PaymentCommand { get; set; }
    [Parameter]
    public EventCallback<PaymentCommand> PaymentCommandChanged { get; set; }

    [Parameter]
    public InvoiceAsPaymentDto? InvoiceInfo { get; set; }

    [Parameter]
    public SaleOrderAsPaymentDto? SaleInfo { get; set; }
}
