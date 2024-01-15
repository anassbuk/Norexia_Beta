using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Stock.Entry;
public partial class StockEntryLinesComponent
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public StockEntryCommand? StockEntryCommand { get; set; }

    [Parameter]
    public EventCallback<StockEntryCommand> StockEntryCommandChanged { get; set; }
    private SfGrid<StockEntryLineDto>? StockLinesGrid;

    private string? productSearchTerm;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;

    protected override void OnInitialized()
    {
        if (StockEntryCommand!.StockEntryLines is null)
            StockEntryCommand!.StockEntryLines = new List<StockEntryLineDto>();

        if (StockEntryCommand!.Type is null)
            StockEntryCommand!.Type = StockRecordType.Partial;
    }

    public void OnActionComplete(ActionEventArgs<StockEntryLineDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            //CalcTotalPrices();
        }
    }

    public async Task SearchProduct(MouseEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(productSearchTerm))
        {
            try
            {
                StockEntryLineDto line = await GCApiProxy!.Proxy.Product_GetProductAsStockEntryLineAsync(productSearchTerm);
                if (StockEntryCommand!.StockEntryLines!.Any(l => l.ProductId == line.ProductId))
                {
                    DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' existe déjà";
                    IsDialogVisible = true;
                }
                else
                {
                    line.Id = Guid.NewGuid();
                    await StockLinesGrid!.AddRecordAsync(line);
                }
            }
            catch (Exception)
            {
                DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' introuvable";
                IsDialogVisible = true;
            }
        }
    }

    readonly List<DropDownStockRecordType> ddStockRecordType = new()
    {
        new DropDownStockRecordType() { DisplayName = "Partiel", Type = StockRecordType.Partial},
        new DropDownStockRecordType() { DisplayName = "Complet", Type = StockRecordType.Complete},
    };

    public class DropDownStockRecordType
    {
        public string? DisplayName { get; set; }
        public StockRecordType Type { get; set; }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }
}
