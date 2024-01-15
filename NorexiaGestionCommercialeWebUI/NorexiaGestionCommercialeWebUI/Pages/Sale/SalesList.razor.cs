using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Notifications;

namespace NorexiaGestionCommercialeWebUI.Pages.Sale;
public partial class SalesList
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    public List<SaleOrderDto>? SaleOrders { get; set; }

    public SfGrid<SaleOrderDto>? SaleOrdersGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };

    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        SaleOrders = (List<SaleOrderDto>)await GCApiProxy!.Proxy.SaleOrder_GetSaleOrdersAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<SaleOrderDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Sales/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            AppStates!.SaleOrder = Args.Data;
            Navigation!.NavigateTo($"/Sales/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                foreach (var item in SaleOrdersGrid!.SelectedRecords)
                {
                    SaleOrders!.Remove(item);
                }
                var toDelete = SaleOrdersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.SaleOrder_DeleteSaleOrderAsync(toDelete);
                await SaleOrdersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Sale order deleted Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }
}
