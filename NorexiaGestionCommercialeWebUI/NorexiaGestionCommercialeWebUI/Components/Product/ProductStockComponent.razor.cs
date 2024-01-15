using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Product;

namespace NorexiaGestionCommercialeWebUI.Components.Product;
public partial class ProductStockComponent
{
    [Parameter]
    public ProductCommand? ProductCommand { get; set; }

    [Parameter]
    public EventCallback<ProductCommand> ProductCommandChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (ProductCommand!.StorageSupplyInfo is null)
        {
            ProductCommand!.StorageSupplyInfo = new();
            await ProductCommandChanged.InvokeAsync(ProductCommand);
        }
    }
}
