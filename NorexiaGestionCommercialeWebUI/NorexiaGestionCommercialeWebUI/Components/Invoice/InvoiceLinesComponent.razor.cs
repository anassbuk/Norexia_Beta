using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Invoice;
public partial class InvoiceLinesComponent
{
    [Parameter]
    public List<PriceGroupDto>? PriceGroups { get; set; }

    [Parameter]
    public List<CurrencyDto>? Currencies { get; set; }

    [Parameter]
    public Guid? DefaultPriceGroupId { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    [Parameter]
    public InvoiceCommand? InvoiceCommand { get; set; }

    [Parameter]
    public EventCallback<InvoiceCommand> InvoiceCommandChanged { get; set; }
    private SfGrid<InvoiceLineDto>? InvoiceLinesGrid;

    private string? productSearchTerm;
    private bool IsDialogVisible;
    private string DialogMessage = string.Empty;

    private double priceExcludingTax = 0;
    private double taxPrice = 0;
    private double priceIncludingTax = 0;
    private double netPrice = 0;

    private double? discount;

    private DepositInvoiceData? depositInvoiceData;

    protected override void OnInitialized()
    {
        if (InvoiceCommand!.InvoiceLines is null)
            InvoiceCommand!.InvoiceLines = new List<InvoiceLineDto>();

        if (InvoiceCommand.Discount != null)
            discount = (double?)InvoiceCommand.Discount / 100;

        CalcTotalPrices();
    }

    protected override void OnParametersSet()
    {
        if (Currencies != null && InvoiceCommand!.CurrencyId is null)
                InvoiceCommand!.CurrencyId = Currencies!.First(c => c.IsDefault).Id;
    }

    public void OnActionComplete(ActionEventArgs<InvoiceLineDto> Args)
    {
        CalcTotalPrices();
    }

    private void CalcTotalPrices()
    {
        if (InvoiceCommand!.InvoiceLines!.Count > 0)
        {
            priceExcludingTax = (double)InvoiceCommand!.InvoiceLines!.Select(s => s.TotalPriceExcludingTax).Sum()!;
            taxPrice = (double)InvoiceCommand!.InvoiceLines!.Select(s => s.TotalPriceExcludingTax * ((double)s.Vat! / 100)).Sum()!;
            priceIncludingTax = (double)InvoiceCommand!.InvoiceLines!.Select(s => s.TotalPriceIncludingTax).Sum()!;
            var discountPrice = priceExcludingTax * (((double?)InvoiceCommand!.Discount ?? 0) / 100);
            netPrice = priceIncludingTax - discountPrice;
            StateHasChanged();
        }
    }

    public void FillDepositInvoiceData()
    {
        if (InvoiceCommand!.PaymentTerms!.DepositInvoiceDownPayment != null)
        {
            CalcTotalPrices();

            var didp = (double)InvoiceCommand!.PaymentTerms!.DepositInvoiceDownPayment / 100;

            depositInvoiceData = new()
            {
                Designation = $"Acompte de {didp:P2} sur la commande N°'{InvoiceCommand!.SaleOrderRef}'",
                Price = priceExcludingTax * didp,
                Qty = 1,
                TotalPriceExcludingTax = priceExcludingTax * didp,
                TotalVATPrice = taxPrice * didp,
                TotalPriceIncludingTax = priceIncludingTax * didp
            };

            StateHasChanged();
        }
    }

    public async Task SearchProduct(MouseEventArgs args)
    {
        if (!string.IsNullOrWhiteSpace(productSearchTerm))
        {
            try
            {
                var line = await GCApiProxy!.Proxy.Product_GetProductAsInvoiceLineAsync(productSearchTerm);
                if (InvoiceCommand!.InvoiceLines!.Any(l => l.ProductId == line.ProductId))
                {
                    DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' existe déjà";
                    IsDialogVisible = true;
                }
                else
                {
                    line.Id = Guid.NewGuid();
                    await InvoiceLinesGrid!.AddRecordAsync(line);
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
        InvoiceCommand!.Discount = ((float?)args.Value ?? 0) * 100;
        CalcTotalPrices();
    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }

    private void LineSellingPriceChanged(ChangeEventArgs<Guid?, PriceGroupDto> args, object context)
    {
        var invoiceLine = (context as InvoiceLineDto);
        if (args.Value != null)
        {
            var selectedPrice = invoiceLine!.SellingPrices.First(l => l.PriceGroupId == args.Value);
            invoiceLine!.SellingPriceId = selectedPrice.Id;
            invoiceLine.Price = selectedPrice.Price;
            invoiceLine.Discount = selectedPrice.Discount ?? 0;
            invoiceLine.Vat = selectedPrice.Vat ?? 0;
            invoiceLine.TotalPriceExcludingTax = invoiceLine.Qty * invoiceLine.Price;
            invoiceLine.TotalVATPrice = invoiceLine.TotalPriceExcludingTax * ((double)invoiceLine.Vat! / 100);
            invoiceLine.TotalPriceIncludingTax = invoiceLine.TotalPriceExcludingTax + invoiceLine.TotalVATPrice;
        }
    }

    public List<PriceGroupDto> GetLinePriceGroups(object context)
    {
        var invoiceLine = (context as InvoiceLineDto);
        var linePriceGroups = PriceGroups!.Where(fg => invoiceLine!.SellingPrices.Any(sg => sg.PriceGroupId == fg.Id)).ToList();
        linePriceGroups.Add(new()
        {
            Id = (Guid)DefaultPriceGroupId!,
            Name = "Par défault"
        });

        return linePriceGroups;
    }

    private void LineQuantityChanged(ChangeEventArgs<int?> args, object context)
    {
        var line = (context as InvoiceLineDto);
        if (args.Value != null)
        {
            line!.TotalPriceExcludingTax = line.Qty * line.Price;
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;
        }
    }
}
