using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Stock.Entry;
public partial class StockEntryForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public StockEntryCommand StockEntryCommand { get; set; } = new();
    private EditContext? EC { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    ToastComponent? Toast;
    protected override void OnInitialized()
    {
        EC = new EditContext(StockEntryCommand);
    }
    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreateStockEntryCommand>(StockEntryCommand);
                await GCApiProxy!.Proxy.StockEntry_CreateStockEntryAsync(createCommand);
                await Toast!.ShowSuccessToast("Stock entry added Successfully");
                Navigation!.NavigateTo("/Stock/Entries");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
