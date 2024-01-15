using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components.Client;
using NorexiaGestionCommercialeWebUI.Models.Provider;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Provider;
public partial class ProviderAddressComponent
{
    [Parameter]
    public ProviderCommand? ProviderCommand { get; set; }

    [Parameter]
    public EventCallback<ProviderCommand> ProviderCommandChanged { get; set; }

    SfGrid<ProviderAddressDto>? AddressGrid;

    readonly List<DropDownAddressType> ddAddressTypes = new()
    {
        new DropDownAddressType() { DisplayName = "Facturation / Livraison", AddressType = AddressType.All},
        new DropDownAddressType() { DisplayName = "Facturation", AddressType = AddressType.Billing},
        new DropDownAddressType() { DisplayName = "Livraison", AddressType = AddressType.Delivery},
    };

    public async Task OnAddressActionBegin(ActionEventArgs<ProviderAddressDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (Args.Action == "Add")
                Args.Data.Id = Guid.NewGuid();
        }
        if (Args.Data.Active == true)
        {
            if (Args.Data.AddressType == AddressType.All)
            {
                if (ProviderCommand!.ProviderAddresses.Any(a => a.Active == true))
                {
                    var subList = ProviderCommand!.ProviderAddresses.Where(a => a.Active == true);
                    foreach (var address in subList)
                    {
                        address.Active = false;
                    }
                }
            }
            else
            {
                if (ProviderCommand!.ProviderAddresses.Any(a => a.Active == true && (a.AddressType == Args.Data.AddressType || a.AddressType == AddressType.All)))
                {
                    var subList = ProviderCommand!.ProviderAddresses.Where(a => a.Active == true && (a.AddressType == Args.Data.AddressType || a.AddressType == AddressType.All));
                    foreach (var address in subList)
                    {
                        address.Active = false;
                    }
                }
            }
            await AddressGrid!.Refresh();
        }
    }
}
