using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Provider;

namespace NorexiaGestionCommercialeWebUI.Components.Delivery.Deliverer;
public partial class DelivererGeneralInformationComponent
{
    [Parameter]
    public DelivererDto? Deliverer { get; set; }

    [Parameter]
    public EventCallback<DelivererDto> DelivererChanged { get; set; }

    protected override void OnInitialized()
    {
        if (Deliverer!.Type is null)
            Deliverer!.Type = DelivererType.Internal;
    }
    private async Task OnTypeChange(ChangeEventArgs args)
    {
        await DelivererChanged.InvokeAsync(Deliverer);
    }
}
