using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Stock.Entry;
public partial class EditStockEntry
{
    [Parameter]
    public Guid Id { get; set; }
    [Inject]
    NavigationManager? Navigation { get; set; }
    public StockEntryCommand StockEntryCommand { get; set; } = new();
    private EditContext? EC { get; set; }

    [Inject]
    public States? AppStates { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    ToastComponent? Toast;

    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(StockEntryCommand);
        if (AppStates!.StockEntry is null)
            Navigation!.NavigateTo("/Stock/Entries");

        StockEntryCommand = Mapper!.Map<StockEntryCommand>(AppStates!.StockEntry);

        var lines = await GCApiProxy!.Proxy.StockEntry_GetStockEntryLinesAsync(Id);
        StockEntryCommand!.StockEntryLines = lines;

        EC = new EditContext(StockEntryCommand);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var command = Mapper!.Map<UpdateStockEntryCommand>(StockEntryCommand);
                await GCApiProxy!.Proxy.StockEntry_UpdateStockEntryAsync(Id, command);
                await Toast!.ShowSuccessToast("Stock entry edited Successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
