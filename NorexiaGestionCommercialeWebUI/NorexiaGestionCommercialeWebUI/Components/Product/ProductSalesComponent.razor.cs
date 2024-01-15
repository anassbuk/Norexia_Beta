using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Product;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel;

namespace NorexiaGestionCommercialeWebUI.Components.Product;
public partial class ProductSalesComponent
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public ProductCommand? ProductCommand { get; set; }

    [Parameter]
    public EventCallback<ProductCommand> ProductCommandChanged { get; set; }

    [Parameter]
    public Guid? DefaultPriceGroupId { get; set; } = new();
    [Parameter]
    public List<PriceGroupDto>? PriceGroups { get; set; } = new();
    PriceGroupDto? SelectedPriceGroup { get; set; } = new();
    List<SellingPrice>? SalePrices { get; set; } = new();
    SellingPrice DefaultSellingPrice { get; set; } = new();

    private EditContext? EC { get; set; }

    private string DialogValidationMessage = string.Empty;

    bool? UsePMP;
    public bool? UsePMPDialog { get; set; }

    protected override void OnInitialized()
    {
        EC = new EditContext(DefaultSellingPrice);
        InitiateComponentData();
    }

    protected override void OnParametersSet()
    {
        DefaultSellingPrice.PriceGroupId = DefaultPriceGroupId;
    }

    public void OnPriceGroupChanged(SelectEventArgs<PriceGroupDto> args)
    {
        SelectedPriceGroup = args.ItemData;
    }


    public void OnSalePriceActionBegin(ActionEventArgs<SellingPrice> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            SelectedPriceGroup = PriceGroups!.Single(p => p.Id == Args.Data.PriceGroupId);
        }

        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (Args.Action == "Add" && SalePrices!.Any(s => s.PriceGroupId == Args.Data.PriceGroupId))
            {
                Args.Cancel = true;
                DialogValidationMessage = "Le groupe de prix sélectionnée déjà utilisée";
            }
            else
            {
                Args.Data.PriceGroupName = SelectedPriceGroup!.Name;
            }
        }
    }

    public void OnSalePriceActionComplete(ActionEventArgs<SellingPrice> args)
    {
        if (args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add) || args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.BeginEdit))
        {
            args.PreventRender = false;
            UsePMPDialog = false;
        }

        DialogValidationMessage = string.Empty;
    }

    public async Task<bool> HandleProductPrices()
    {
        if (EC!.Validate())
        {
            List<SellingPriceDto> sellingPrices = new();
            sellingPrices.Add(new SellingPriceDto()
            {
                PriceGroupId = DefaultSellingPrice.PriceGroupId,
                Discount = (int?)(DefaultSellingPrice.Discount * 100),
                Price = DefaultSellingPrice.Price,
                Margin = DefaultSellingPrice.Margin,
                Vat = (int?)(DefaultSellingPrice.Vat * 100),
            });

            sellingPrices.AddRange(SalePrices!.Select(u => new SellingPriceDto()
            {
                PriceGroupId = u.PriceGroupId,
                Discount = (int?)(u.Discount * 100),
                Price = u.Price,
                Margin = u.Margin,
                Vat = (int?)(u.Vat * 100),
            }).ToList());

            ProductCommand!.SellingPrices = sellingPrices;
            await ProductCommandChanged.InvokeAsync(ProductCommand);
            return true;
        }
        else
            return false;

    }

    public void InitiateComponentData()
    {
        if (ProductCommand!.SellingPrices != null)
        {
            List<SellingPriceDto> sellingPricesDto = new(ProductCommand!.SellingPrices);
            var defaultSellingPrice = sellingPricesDto.FirstOrDefault(p => p.PriceGroupId == DefaultPriceGroupId);
            if (defaultSellingPrice != null)
            {
                sellingPricesDto.Remove(defaultSellingPrice);
                DefaultSellingPrice.Price = defaultSellingPrice.Price;
                DefaultSellingPrice.Margin = defaultSellingPrice.Margin;
                DefaultSellingPrice.Discount = defaultSellingPrice.Discount != null ? (double)defaultSellingPrice.Discount / 100 : null;
                DefaultSellingPrice.Vat = defaultSellingPrice.Vat != null ? (double)defaultSellingPrice.Vat / 100 : null;
                DefaultSellingPrice.PurchasePrice = defaultSellingPrice.Price - defaultSellingPrice.Margin;
            }

            List<SellingPrice> sellingPrices = new();
            for (int i = 0; i < sellingPricesDto.Count; i++)
            {
                var productSellingPrice = sellingPricesDto.ToList()[i];
                SellingPrice sellingPrice = new();
                sellingPrice.PriceGroupId = productSellingPrice.PriceGroupId;

                sellingPrice.PriceGroupName = PriceGroups!.First(g => g.Id == productSellingPrice.PriceGroupId).Name;

                sellingPrice.Price = productSellingPrice.Price;
                sellingPrice.Margin = productSellingPrice.Margin;
                sellingPrice.Discount = productSellingPrice.Discount != null ? (double)productSellingPrice.Discount / 100 : null;
                sellingPrice.Vat = productSellingPrice.Vat != null ? (double)productSellingPrice.Vat / 100 : null;
                sellingPrice.PurchasePrice = productSellingPrice.Price - productSellingPrice.Margin;

                sellingPrices!.Add(sellingPrice);
            }

            SalePrices = sellingPrices;
        }
    }

    public void OnUsePMPChanged(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
    {
        //TODO : Calculate PMP
        DefaultSellingPrice.PurchasePrice = 100.00;
    }
    private void PurchasePriceValueChanged(ChangeEventArgs<double?> args)
    {
        if (DefaultSellingPrice.PurchasePrice != null)
            if (DefaultSellingPrice.Price != null)
                DefaultSellingPrice.Margin = DefaultSellingPrice.Price - DefaultSellingPrice.PurchasePrice;
            else if (DefaultSellingPrice!.Margin != null)
                DefaultSellingPrice.Price = DefaultSellingPrice.Margin + DefaultSellingPrice.PurchasePrice;
    }

    private void PriceValueChanged(ChangeEventArgs<double?> args)
    {
        if (DefaultSellingPrice.Price != null && DefaultSellingPrice.PurchasePrice != null)
            DefaultSellingPrice.Margin = DefaultSellingPrice.Price - DefaultSellingPrice.PurchasePrice;
    }

    private void MarginValueChanged(ChangeEventArgs<double?> args)
    {
        if (DefaultSellingPrice.Margin != null && DefaultSellingPrice.PurchasePrice != null)
            DefaultSellingPrice.Price = DefaultSellingPrice.Margin + DefaultSellingPrice.PurchasePrice;
    }

    public void OnDialogUsePMPChanged(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args, object context)
    {
        var DialogSellingPrice = context as SellingPrice;
        DialogSellingPrice!.PurchasePrice = 100;
    }
    private void DialogPurchasePriceValueChanged(ChangeEventArgs<double?> args, object context) 
    {
        var DialogSellingPrice = context as SellingPrice;
        if (DialogSellingPrice!.PurchasePrice != null)
            if (DialogSellingPrice.Price != null)
                DialogSellingPrice.Margin = DialogSellingPrice.Price - DialogSellingPrice.PurchasePrice;
            else if (DialogSellingPrice!.Margin != null)
                DialogSellingPrice.Price = DialogSellingPrice.Margin + DialogSellingPrice.PurchasePrice;
    }

    private void DialogPriceValueChanged(ChangeEventArgs<double?> args, object context)
    {
        var DialogSellingPrice = context as SellingPrice;
        if (DialogSellingPrice!.Price != null && DialogSellingPrice.PurchasePrice != null)
            DialogSellingPrice.Margin = DialogSellingPrice.Price - DialogSellingPrice.PurchasePrice;
    }

    private void DialogMarginValueChanged(ChangeEventArgs<double?> args, object context)
    {
        var DialogSellingPrice = context as SellingPrice;
        if (DialogSellingPrice!.Margin != null && DialogSellingPrice.PurchasePrice != null)
            DialogSellingPrice.Price = DialogSellingPrice.Margin + DialogSellingPrice.PurchasePrice;
    }

    public class SellingPrice : INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        private string? priceGroupName;
        public string? PriceGroupName
        {
            get { return priceGroupName; }
            set
            {
                this.priceGroupName = value;
                NotifyPropertyChanged(nameof(PriceGroupName));
            }
        }
        private Guid? priceGroupId;
        public Guid? PriceGroupId
        {
            get { return priceGroupId; }
            set
            {
                this.priceGroupId = value;
                NotifyPropertyChanged(nameof(PriceGroupId));
            }
        }
        private double? price { get; set; }
        public double? Price
        {
            get { return price; }
            set
            {
                this.price = value;
                NotifyPropertyChanged(nameof(Price));
            }
        }
        private double? purchasePrice;

        public double? PurchasePrice
        {
            get { return purchasePrice; }
            set
            {
                this.purchasePrice = value;
                NotifyPropertyChanged(nameof(PurchasePrice));
            }
        }
        private double? margin;

        public double? Margin
        {
            get { return margin; }
            set
            {
                this.margin = value;
                NotifyPropertyChanged(nameof(margin));
            }
        }
        public double? Vat { get; set; }
        public double? Discount { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
