using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.Models.Delivery;
using NorexiaGestionCommercialeWebUI.Components.Delivery;
using Syncfusion.Blazor.Navigations;

namespace NorexiaGestionCommercialeWebUI.Pages.Delivery;
public partial class DeliveryForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public DeliveryCommand DeliveryCommand { get; set; } = new();
    public CustomerDetailsDto Customer { get; set; } = new();
    private EditContext? EC { get; set; }
    private List<PriceGroupDto>? PriceGroups;
    private Guid? DefaultPriceGroupId;
    private DeliveryInfoComponent? DeliveryInfoComponent;

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        EC = new EditContext(DeliveryCommand);
        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();
    }
    public async Task Save()
    {

        try
        {
        

            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreateDeliveryCommand>(DeliveryCommand);
                await GCApiProxy!.Proxy.Delivery_CreateDeliveryAsync(createCommand);
                await Toast!.ShowSuccessToast("Delivery added Successfully");
                Navigation!.NavigateTo("/Deliveries");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }

    private void DisplayCustomerAddress()
    {
        DeliveryInfoComponent!.DisplayCustomerAddress();
    }
}
