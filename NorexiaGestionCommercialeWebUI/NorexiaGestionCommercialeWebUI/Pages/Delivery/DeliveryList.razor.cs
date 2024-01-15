using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Pages.Delivery;
public partial class DeliveryList
{
    [Inject]
    NavigationManager? Navigation { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    public List<DeliveryDto>? Deliveries { get; set; }

    public SfGrid<DeliveryDto>? DeliveriesGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };
    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        Deliveries = (List<DeliveryDto>)await GCApiProxy!.Proxy.Delivery_GetDeliveriesAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<DeliveryDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Deliveries/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            AppStates!.Delivery = Args.Data;
            Navigation!.NavigateTo($"/Deliveries/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                var toDelete = DeliveriesGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Delivery_DeleteDeliveryAsync(toDelete);
                foreach (var item in DeliveriesGrid!.SelectedRecords)
                {
                    Deliveries!.Remove(item);
                }
                await DeliveriesGrid!.Refresh();
                await Toast!.ShowSuccessToast("Delivery deleted Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }
}
