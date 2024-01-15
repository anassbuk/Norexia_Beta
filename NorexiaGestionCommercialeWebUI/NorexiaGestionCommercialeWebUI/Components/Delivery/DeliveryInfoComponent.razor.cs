using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Delivery;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Components.Delivery;
public partial class DeliveryInfoComponent
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public DeliveryCommand? DeliveryCommand { get; set; }
    [Parameter]
    public EventCallback<DeliveryCommand> DeliveryCommandChanged { get; set; }
    public DelivererDto? Deliverer { get; set; }
    [Parameter]
    public CustomerDetailsDto? Customer { get; set; }
    [Parameter]
    public EventCallback<CustomerDetailsDto> CustomerChanged { get; set; }
    private CustomerAddressDto? DeliveryCustomerAddress;
    private string? delivererSearchTerm;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;

    protected override void OnParametersSet()
    {
        delivererSearchTerm = DeliveryCommand!.DelivererRef;
    }

    public async Task SearchDeliverer(MouseEventArgs args)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(delivererSearchTerm))
            {
                Deliverer = await GCApiProxy!.Proxy.Deliverer_GetDelivererByReferenceOrNameAsync(delivererSearchTerm);
                DisplayDeliverer();
            }

        }
        catch (Exception)
        {
            DialogMessage = $"Livreur avec le terme de recherche '{delivererSearchTerm}' introuvable";
            IsDialogVisible = true;
        }
    }

    public void DisplayCustomerAddress()
    {
        if (Customer != null && Customer.CustomerAddresses != null)
        {
            DeliveryCustomerAddress = Customer.CustomerAddresses.FirstOrDefault(a => (a.AddressType != AddressType.Delivery || a.AddressType != AddressType.All) && a.Active == true);
            StateHasChanged();
        }
    }

    private void DisplayDeliverer()
    {
        if (Deliverer != null)
        {
            DeliveryCommand!.DelivererId = Deliverer.Id;
            delivererSearchTerm = $"{Deliverer.Reference} - {Deliverer.FirstName}, {Deliverer.LastName}";
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    readonly List<DropDownDeliveryMode> ddDeliveryMode = new()
        {
            new DropDownDeliveryMode() { DisplayName = "Récupération au magasin", DeliveryMode = DeliveryMode.PickUpAtStore},
            new DropDownDeliveryMode() { DisplayName = "Livraison à domicile", DeliveryMode = DeliveryMode.HomeDelivery},
        };

    public class DropDownDeliveryMode
    {
        public string? DisplayName { get; set; }
        public DeliveryMode DeliveryMode { get; set; }
    }
}
