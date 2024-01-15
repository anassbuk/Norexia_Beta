using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Sales;
public partial class SalesLinesComponent
{
    [Parameter]
    public SaleCommand? SaleCommand { get; set; }

    [Parameter]
    public EventCallback<SaleCommand> SaleCommandChanged { get; set; }

    [Parameter]
    public List<PriceGroupDto>? PriceGroups { get; set; }
    [Parameter]
    public Guid? DefaultPriceGroupId { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    private string? productSearchTerm;
    private SfGrid<SaleOrderLineDto>? SaleLinesGrid;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;

    private int nbrProduct = 0;
    private int nbrPieces = 0;
    private double priceExcludingTax = 0;
    private double taxPrice = 0;
    private double priceIncludingTax = 0;
    private double discountPrice = 0;
    private double netPrice = 0;

    private double? discount;

    protected override void OnInitialized()
    {
        if (SaleCommand!.SaleOrderLines is null)
            SaleCommand!.SaleOrderLines = new List<SaleOrderLineDto>();
        if (SaleCommand.Discount != null)
            discount = (double?)SaleCommand.Discount / 100;
        CalcTotalPrices();
    }

    public void OnActionComplete(ActionEventArgs<SaleOrderLineDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            CalcTotalPrices();
        }
    }

    public async Task SearchProduct(MouseEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(productSearchTerm))
        {
            try
            {
                SaleOrderLineDto line = await GCApiProxy!.Proxy.Product_GetProductAsSellOrderLineAsync(productSearchTerm);
                if (SaleCommand!.SaleOrderLines!.Any(l => l.ProductId == line.ProductId))
                {
                    var existLine = SaleCommand!.SaleOrderLines!.First(l => l.ProductId == line.ProductId);
                    existLine.Qty += 1;

                    existLine.TotalPriceExcludingTax = existLine.Qty * (existLine.Price - (existLine.Price * ((double)existLine.Discount! / 100)));
                    existLine.TotalVATPrice = existLine.TotalPriceExcludingTax + (existLine.Price * ((double?)existLine.Vat ?? 0 / 100));
                    await SaleLinesGrid!.Refresh();
                }
                else
                {
                    line.Id = Guid.NewGuid();
                    await SaleLinesGrid!.AddRecordAsync(line);
                }
            }
            catch (Exception)
            {
                DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' introuvable";
                IsDialogVisible = true;
            }
        }
    }
    private void DiscountValueChangeHandler(ChangeEventArgs<double?> args)
    {
        SaleCommand!.Discount = ((float?)args.Value ?? 0) * 100;
        CalcTotalPrices();
    }
    private void CalcTotalPrices()
    {
        if(SaleCommand!.SaleOrderLines!.Count > 0)
        {
            nbrProduct = SaleCommand!.SaleOrderLines!.Count;
            nbrPieces = (int)SaleCommand!.SaleOrderLines!.Select(s => s.Qty).Sum()!;
            priceExcludingTax = (double)SaleCommand!.SaleOrderLines!.Select(s => s.TotalPriceExcludingTax).Sum()!;
            taxPrice = (double)SaleCommand!.SaleOrderLines!.Select(s => s.TotalVATPrice).Sum()!;
            priceIncludingTax = (double)SaleCommand!.SaleOrderLines!.Select(s => s.TotalPriceIncludingTax).Sum()!;
            discountPrice = priceExcludingTax * (((double?)SaleCommand!.Discount ?? 0) / 100);
            netPrice = priceIncludingTax - discountPrice;
            StateHasChanged();
        }
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    private void LineSellingPriceChanged(ChangeEventArgs<Guid?, PriceGroupDto> args, object context)
    {
        var line = (context as SaleOrderLineDto);
        if (args.Value != null)
        {
            var selectedPrice = line!.SellingPrices.First(l => l.PriceGroupId == args.Value);
            line!.SellingPriceId = selectedPrice.Id;
            line.Price = selectedPrice.Price;
            line.Discount = selectedPrice.Discount ?? 0;
            line.Vat = selectedPrice.Vat ?? 0;
            line.TotalPriceExcludingTax = line.Qty * line.Price;
            line.TotalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;
        }
    }

    public List<PriceGroupDto> GetLinePriceGroups(object context)
    {
        var line = (context as SaleOrderLineDto);
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
        var line = (context as SaleOrderLineDto);
        if (args.Value != null)
        {
            line!.TotalPriceExcludingTax = line.Qty * line.Price;
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;
        }
    }
}
