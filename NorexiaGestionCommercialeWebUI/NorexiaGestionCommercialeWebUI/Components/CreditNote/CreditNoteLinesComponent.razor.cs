using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.CreditNote;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using NorexiaGestionCommercialeWebUI.Pages.Invoice;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.CreditNote
{
    public partial class CreditNoteLinesComponent
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
        public CreditNoteCommand? CreditNoteCommand { get; set; }

        [Parameter]
        public EventCallback<CreditNoteCommand> CreditNoteCommandChanged { get; set; }
        private SfGrid<CreditNoteLineDto>? CreditNoteLinesGrid;

        private string? productSearchTerm;
        private bool IsDialogVisible;
        private string DialogMessage = string.Empty;

        private double priceExcludingTax = 0;
        private double taxPrice = 0;
        private double priceIncludingTax = 0;
        private double netPrice = 0;
       
       

        private double? discount;



        protected override void OnInitialized()
        {
            if (CreditNoteCommand!.CreditNoteLines is null)
                CreditNoteCommand!.CreditNoteLines = new List<CreditNoteLineDto>();

            if (CreditNoteCommand.Discount != null)
                discount = (double?)CreditNoteCommand.Discount / 100;

            CalcTotalPrices();
        }

        public void OnActionComplete(ActionEventArgs<CreditNoteLineDto> Args)
        {
            CalcTotalPrices();
        }
        private void CalcTotalPrices()
        {
            if (CreditNoteCommand!.CreditNoteLines!.Count > 0)
            {
                priceExcludingTax = (double)CreditNoteCommand!.CreditNoteLines!.Select(s => s.TotalPriceExcludingTax).Sum()!;
                taxPrice = (double)CreditNoteCommand!.CreditNoteLines!.Select(s => s.TotalPriceExcludingTax * ((double)s.Vat! / 100)).Sum()!;
                priceIncludingTax = (double)CreditNoteCommand!.CreditNoteLines!.Select(s => s.TotalPriceIncludingTax).Sum()!;
                var discountPrice = priceExcludingTax * (((double?)CreditNoteCommand!.Discount ?? 0) / 100);
                netPrice = priceIncludingTax - discountPrice;
              
                StateHasChanged();

            }
            
        }
      

        private void DialogOkClick()
        {
            this.IsDialogVisible = false;
        }

       
        private  void LineQuantityChanged(ChangeEventArgs<int?> args, object context)
        {
            var line = (context as CreditNoteLineDto);
            if (args.Value != null)
            {
                line!.TotalPriceExcludingTax = line.Qty * line.Price;
                var totalVATPrice = line.TotalPriceExcludingTax * ((double)line.Vat! / 100);
                line.TotalPriceIncludingTax = line.TotalPriceExcludingTax + totalVATPrice;
            }
        }

        public List<PriceGroupDto> GetLinePriceGroups(object context)
        {
            var creditNoteLineDto = (context as CreditNoteLineDto);
            var linePriceGroups = PriceGroups!.Where(fg => creditNoteLineDto!.SellingPrices.Any(sg => sg.PriceGroupId == fg.Id)).ToList();
            linePriceGroups.Add(new()
            {
                Id = (Guid)DefaultPriceGroupId!,
                Name = "Par défault"
            });

            return linePriceGroups;
        }


    



    }
}
