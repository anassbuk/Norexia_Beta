using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Delivery;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Delivery;
public partial class DeliveryLinesComponent
{
    [Parameter]
    public List<PriceGroupDto>? PriceGroups { get; set; }

    [Parameter]
    public Guid? DefaultPriceGroupId { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    [Parameter]
    public DeliveryCommand? DeliveryCommand { get; set; }

    [Parameter]
    public EventCallback<DeliveryCommand> DeliveryCommandChanged { get; set; }
    private SfGrid<DeliveryLineDto>? DeliveryLinesGrid;

    private string? productSearchTerm;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;
    private double netPrice = 0;

    protected override void OnInitialized()
    {
        if (DeliveryCommand!.DeliveryLines is null)
            DeliveryCommand!.DeliveryLines = new List<DeliveryLineDto>();

        if (DeliveryCommand!.Type is null)
            DeliveryCommand!.Type = StockRecordType.Partial;


        CalcTotalPrices();
    }

    public void OnActionComplete(ActionEventArgs<DeliveryLineDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            CalcTotalPrices();
        }
    }

    private void CalcTotalPrices()
    {
        if (DeliveryCommand!.DeliveryLines!.Count > 0)
        {
            netPrice = DeliveryCommand!.DeliveryLines.Select(l=>l.TotalPriceIncludingTax).Sum() ?? 0;
            StateHasChanged();
        }
    }
    public async Task SearchProduct(MouseEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(productSearchTerm))
        {
            try
            {
                var line = await GCApiProxy!.Proxy.Product_GetProductAsDeliveryLineAsync(productSearchTerm);
                if (DeliveryCommand!.DeliveryLines!.Any(l => l.ProductId == line.ProductId))
                {
                    DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' existe déjà";
                    IsDialogVisible = true;
                }
                else
                {
                    line.Id = Guid.NewGuid();
                    await DeliveryLinesGrid!.AddRecordAsync(line);
                }
            }
            catch (Exception)
            {
                DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' introuvable";
                IsDialogVisible = true;
            }
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    private void LineSellingPriceChanged(ChangeEventArgs<Guid?, PriceGroupDto> args, object context)
    {
        var line = (context as DeliveryLineDto);
        if (args.Value != null)
        {
            var selectedPrice = line!.SellingPrices.First(l => l.PriceGroupId == args.Value);
            line!.SellingPriceId = selectedPrice.Id;
            line.UnitPrice = selectedPrice.Price;
            line.Discount = selectedPrice.Discount ?? 0;
            line.Vat = selectedPrice.Vat ?? 0;
            line.TotalPriceExcludingTax = line.Qty * line.UnitPrice;
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;
        }
    }

    public List<PriceGroupDto> GetLinePriceGroups(object context)
    {
        var line = (context as DeliveryLineDto);
        var linePriceGroups = PriceGroups!.Where(fg => line!.SellingPrices.Any(sg => sg.PriceGroupId == fg.Id)).ToList();
        linePriceGroups.Add(new()
        {
            Id = (Guid)DefaultPriceGroupId!,
            Name = "Par défault"
        });

        return linePriceGroups;
    }

    private void LineQuantityChanged(ChangeEventArgs<int?> args, object context)
    {
        var line = (context as DeliveryLineDto);
        if (args.Value != null)
        {
            line!.TotalPriceExcludingTax = line.Qty * line.UnitPrice;
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;
        }
    }
}
