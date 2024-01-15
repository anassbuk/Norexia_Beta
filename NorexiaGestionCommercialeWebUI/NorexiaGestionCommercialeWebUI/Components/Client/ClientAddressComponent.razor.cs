using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components.Product;
using NorexiaGestionCommercialeWebUI.Models.Client;
using Syncfusion.Blazor.Grids;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductClassificationComponent;

namespace NorexiaGestionCommercialeWebUI.Components.Client;
public partial class ClientAddressComponent
{
    [Parameter]
    public ClientCommand? ClientCommand { get; set; }

    [Parameter]
    public EventCallback<ClientCommand> ClientCommandChanged { get; set; }

    SfGrid<CustomerAddressDto>? AddressGrid;

    readonly List<DropDownAddressType> ddAddressTypes = new()
    {
        new DropDownAddressType() { DisplayName = "Facturation / Livraison", AddressType = AddressType.All},
        new DropDownAddressType() { DisplayName = "Facturation", AddressType = AddressType.Billing},
        new DropDownAddressType() { DisplayName = "Livraison", AddressType = AddressType.Delivery},
    };

    public async Task OnAddressActionBegin(ActionEventArgs<CustomerAddressDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (Args.Action == "Add")
            {
                Args.Data.Id = Guid.NewGuid();
            }

            if (Args.Data.Active == true)
            {
                if (Args.Data.AddressType == AddressType.All)
                {
                    if (ClientCommand!.CustomerAddresses.Any(a => a.Active == true))
                    {
                        var subList = ClientCommand!.CustomerAddresses.Where(a => a.Active == true);
                        foreach (var address in subList)
                        {
                            address.Active = false;
                        }
                    }
                }
                else
                {
                    if (ClientCommand!.CustomerAddresses.Any(a => a.Active == true && (a.AddressType == Args.Data.AddressType || a.AddressType == AddressType.All)))
                    {
                        var subList = ClientCommand!.CustomerAddresses.Where(a => a.Active == true && (a.AddressType == Args.Data.AddressType || a.AddressType == AddressType.All));
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
}

public class DropDownAddressType
{
    public string? DisplayName { get; set; }
    public AddressType AddressType { get; set; }
}
