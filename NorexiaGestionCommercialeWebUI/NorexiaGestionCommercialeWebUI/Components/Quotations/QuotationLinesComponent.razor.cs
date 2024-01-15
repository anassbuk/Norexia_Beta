using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Quotation;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Quotations;

    public partial class QuotationLinesComponent
    {
        [Parameter]
        public QuotationCommand? QuotationCommand { get; set; }

        [Parameter]
        public EventCallback<QuotationCommand> QuotationCommandChanged { get; set; }

        [Parameter]
        public List<PriceGroupDto>? PriceGroups { get; set; }
        [Parameter]
        public Guid? DefaultPriceGroupId { get; set; }
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }

        private string? productSearchTerm;
        private SfGrid<QuotationLineDto>? QuotationLinesGrid;
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
            if (QuotationCommand!.QuotationLines is null)
                QuotationCommand!.QuotationLines = new List<QuotationLineDto>();
            if (QuotationCommand.Discount != null)
                discount = (double?)QuotationCommand.Discount / 100;
            CalcTotalPrices();
        }

        public void OnActionComplete(ActionEventArgs<QuotationLineDto> Args)
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
                    QuotationLineDto line = await GCApiProxy!.Proxy.Quotation_GetProductAsQuotationLineAsync(productSearchTerm);
                    if(QuotationCommand!.QuotationLines!.Any(l =>l.ProductId == line.ProductId))
                    {
                        var existingLine = QuotationCommand!.QuotationLines!.First(l => l.ProductId == line.ProductId);
                        existingLine.Qty += 1;

                        existingLine.TotalPriceExcludingTax = existingLine.Qty * (existingLine.Price - (existingLine.Price * ((double)existingLine.Discount! / 100)));
                        existingLine.TotalPriceIncludingTax = existingLine.TotalPriceExcludingTax + (existingLine.TotalPriceExcludingTax * (existingLine.Vat / 100));
                        await QuotationLinesGrid!.Refresh();

                    }
                    else
                    {
                        line.Id = Guid.NewGuid();
                        await QuotationLinesGrid!.AddRecordAsync(line);
                        
                    }

                }catch(Exception)
                {
                    DialogMessage = $"Produit avec le terme de recherche '{productSearchTerm}' introuvable"; ;
                    IsDialogVisible = true;
                }
            }

        }

        private void DiscountValueChangeHandler(ChangeEventArgs<double?> args)
        {
            QuotationCommand!.Discount = ((float?)args.Value ?? 0) * 100;
            CalcTotalPrices();
        }

        public void CalcTotalPrices()
        {
            if (QuotationCommand!.QuotationLines!.Count >0)
            {
                nbrProduct = QuotationCommand!.QuotationLines!.Count;
                nbrPieces = (int)QuotationCommand!.QuotationLines!.Select(s => s.Qty).Sum()!;
                priceExcludingTax = (double)QuotationCommand!.QuotationLines!.Select(s => s.TotalPriceExcludingTax).Sum()!;
                taxPrice = (double)QuotationCommand!.QuotationLines!.Select(s => s.TotalPriceIncludingTax).Sum()!;
                priceIncludingTax = (double)QuotationCommand!.QuotationLines!.Select(s => s.TotalPriceIncludingTax).Sum()!;
                discountPrice = priceExcludingTax * (((double?)QuotationCommand!.Discount ?? 0) / 100);
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
            var quotationLine = (context as QuotationLineDto);
            if (args.Value != null)
        {
                var selectedPrice = quotationLine!.SellingPrices.First(l => l.PriceGroupId == args.Value);
                quotationLine!.SellingPriceId = selectedPrice.Id;
                quotationLine.Price = selectedPrice.Price;
                quotationLine.Discount = selectedPrice.Discount ?? 0;
                quotationLine.Vat = selectedPrice.Vat ?? 0;
                quotationLine.TotalPriceExcludingTax = quotationLine.Qty * quotationLine.Price;
                quotationLine.TotalVATPrice = quotationLine.TotalPriceExcludingTax * ((double)quotationLine.Vat! / 100);
                quotationLine.TotalPriceIncludingTax = quotationLine.TotalPriceExcludingTax + quotationLine.TotalVATPrice;
            }
        }
        
        public List<PriceGroupDto> GetLinePriceGroups(object context)
         {
          var line =(context as QuotationLineDto);
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
        var line = (context as QuotationLineDto);
        if (args.Value != null)
        {
            line!.TotalPriceExcludingTax = line.Qty * line.Price;
            var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
            line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;
        }
    }
}

