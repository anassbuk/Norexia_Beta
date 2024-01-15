using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Purchase;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Purchases;
public partial class PurchasesLinesComponent
{
    [Parameter]
    public PurchaseCommand? PurchaseCommand { get; set; }

    [Parameter]
    public EventCallback<PurchaseCommand> PurchaseCommandChanged { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    private string? productSearchTerm;
    private SfGrid<PurchaseOrderLineDto>? PurchaseLinesGrid;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;

    private int nbrProduct = 0;
    private int nbrPieces = 0;
    private double priceExcludingTax = 0;
    private double taxPrice = 0;
    private double priceIncludingTax = 0;
    private double netPrice = 0;

    protected override void OnInitialized()
    {
        if (PurchaseCommand!.PurchaseOrderLines is null)
            PurchaseCommand!.PurchaseOrderLines = new List<PurchaseOrderLineDto>();
        CalcTotalPrices();
    }

    public void OnActionComplete(ActionEventArgs<PurchaseOrderLineDto> Args)
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
                PurchaseOrderLineDto line = await GCApiProxy!.Proxy.Product_GetProductAsPurchaseOrderLineAsync(productSearchTerm);
                if (PurchaseCommand!.PurchaseOrderLines!.Any(l => l.ProductId == line.ProductId))
                {
                    var existLine = PurchaseCommand!.PurchaseOrderLines!.First(l => l.ProductId == line.ProductId);
                    existLine.Qty += 1;

                    existLine.TotalPriceExcludingTax = existLine.Qty * existLine.Price;
                    existLine.TotalVATPrice = existLine.TotalPriceExcludingTax + (existLine.Price * ((double?)existLine.Vat ?? 0 / 100));
                    await PurchaseLinesGrid!.Refresh();
                }
                else
                {
                    line.Id = Guid.NewGuid();
                    await PurchaseLinesGrid!.AddRecordAsync(line);
                }
            }
            catch (Exception)
            {
                DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' introuvable";
                IsDialogVisible = true;
            }
        }
    }

    private void LinePriceChanged(ChangeEventArgs<double?> args, object context)
    {
        var line = (context as PurchaseOrderLineDto);
        if (args.Value != null)
        {
            line!.TotalPriceExcludingTax = line.Qty * line.Price;
            line.TotalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;
        }
    }
    private void LineQtyChanged(ChangeEventArgs<int?> args, object context)
    {
        var line = (context as PurchaseOrderLineDto);
        if (args.Value != null)
        {
            line!.TotalPriceExcludingTax = line.Qty * line.Price;
            line.TotalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;
        }
    }
    private void LineVatChanged(ChangeEventArgs<double?> args, object context)
    {
        var line = (context as PurchaseOrderLineDto);
        double? currentVat = ((double?)line!.Vat ?? 0) / 100;
        if (args.Value != null)
        {
            line!.Vat = (int)(args.Value * 100);
            line.TotalPriceExcludingTax = line.Qty * line.Price;
            line.TotalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + line.TotalVATPrice;
        }
    }
    private void CalcTotalPrices()
    {
        if (PurchaseCommand!.PurchaseOrderLines!.Count > 0)
        {
            nbrProduct = PurchaseCommand!.PurchaseOrderLines!.Count();
            nbrPieces = (int)PurchaseCommand!.PurchaseOrderLines!.Select(s => s.Qty).Sum()!;
            priceExcludingTax = (double)PurchaseCommand!.PurchaseOrderLines!.Select(s => s.TotalPriceExcludingTax).Sum()!;
            taxPrice = (double)PurchaseCommand!.PurchaseOrderLines!.Select(s => s.TotalVATPrice).Sum()!;
            priceIncludingTax = (double)PurchaseCommand!.PurchaseOrderLines!.Select(s => s.TotalPriceIncludingTax).Sum()!;
            netPrice = priceIncludingTax;
            StateHasChanged();
        }
    }

    private void DialogOkClick()
    {
        IsDialogVisible = false;
    }
}
