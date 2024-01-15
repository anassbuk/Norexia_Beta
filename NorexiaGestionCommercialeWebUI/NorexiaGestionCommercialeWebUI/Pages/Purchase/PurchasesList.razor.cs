using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;

namespace NorexiaGestionCommercialeWebUI.Pages.Purchase;
public partial class PurchasesList
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    public List<PurchaseOrderDto>? PurchaseOrders { get; set; }

    public SfGrid<PurchaseOrderDto>? PurchaseOrdersGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };

    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        PurchaseOrders = (List<PurchaseOrderDto>)await GCApiProxy!.Proxy.PurchaseOrder_GetPurchaseOrdersAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<PurchaseOrderDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Purchases/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            AppStates!.PurchaseOrder = Args.Data;
            Navigation!.NavigateTo($"/Purchases/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                foreach (var item in PurchaseOrdersGrid!.SelectedRecords)
                {
                    PurchaseOrders!.Remove(item);
                }
                var toDelete = PurchaseOrdersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.PurchaseOrder_DeletePurchaseOrderAsync(toDelete);
                await PurchaseOrdersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Purchase order deleted Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }
}
