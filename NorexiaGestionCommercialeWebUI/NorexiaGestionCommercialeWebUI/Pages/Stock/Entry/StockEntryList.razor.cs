using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Notifications;

namespace NorexiaGestionCommercialeWebUI.Pages.Stock.Entry;
public partial class StockEntryList
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    public List<StockEntryDto>? StockEntries { get; set; }

    public SfGrid<StockEntryDto>? StockEntriesGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };
    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        StockEntries = (List<StockEntryDto>)await GCApiProxy!.Proxy.StockEntry_GetStockEntriesAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<StockEntryDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Stock/Entries/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            AppStates!.StockEntry = Args.Data;
            Navigation!.NavigateTo($"/Stock/Entries/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                var toDelete = StockEntriesGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.PurchaseOrder_DeletePurchaseOrderAsync(toDelete);
                foreach (var item in StockEntriesGrid!.SelectedRecords)
                {
                    StockEntries!.Remove(item);
                }
                await StockEntriesGrid!.Refresh();
                await Toast!.ShowSuccessToast("Stock entry deleted Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }
}