using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Models.Delivery;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;
using NorexiaGestionCommercialeWebUI.Components.Delivery;

namespace NorexiaGestionCommercialeWebUI.Pages.Delivery;
public partial class EditDelivery
{
    [Parameter]
    public Guid Id { get; set; }
    [Inject]
    NavigationManager? Navigation { get; set; }
    public DeliveryCommand DeliveryCommand { get; set; } = new();
    private EditContext? EC { get; set; }
    private List<PriceGroupDto>? PriceGroups;
    private Guid? DefaultPriceGroupId;
    private DeliveryInfoComponent? DeliveryInfoComponent;
    public CustomerDetailsDto Customer { get; set; } = new();

    [Inject]
    public IMapper? Mapper { get; set; }

    [Inject]
    public States? AppStates { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        EC = new EditContext(DeliveryCommand);
        if (AppStates!.Delivery is null)
            Navigation!.NavigateTo("/Deliveries");

        DeliveryCommand = Mapper!.Map<DeliveryCommand>(AppStates!.Delivery);

        var lines = await GCApiProxy!.Proxy.Delivery_GetDeliveryLinesAsync(Id);
        DeliveryCommand!.DeliveryLines = lines;

        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();

        EC = new EditContext(DeliveryCommand);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var updateCommand = Mapper!.Map<UpdateDeliveryCommand>(DeliveryCommand);
                await GCApiProxy!.Proxy.Delivery_UpdateDeliveryAsync(Id, updateCommand);
                await Toast!.ShowSuccessToast("Delivery Edited Successfully");
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
