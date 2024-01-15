using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace NorexiaGestionCommercialeWebUI.Pages.Product;
public partial class ProductList
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<ProductDto>? Products { get; set; }

    public SfGrid<ProductDto>? ProductsGrid { get; set; }


    private List<Object> Toolbaritems = new() { "Add", "Edit", "Delete", new ItemModel() { Text = "Activer / Désactiver", TooltipText="", PrefixIcon = "e-circle-check", Id = "Active" } };

    ToastComponent? Toast;

    protected async override Task OnInitializedAsync()
    {
        Products = (List<ProductDto>?)await GCApiProxy!.Proxy.Product_GetProductsAsync();
    }

    public async Task OnProductActionBegin(ActionEventArgs<ProductDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Products/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo($"/Products/{Args.Data.Id}/{Args.Data.ShortDesignation}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                foreach (var item in ProductsGrid!.SelectedRecords)
                {
                    Products!.Remove(item);
                }
                var productsToDelete = ProductsGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Product_DeleteProductAsync(productsToDelete);
                await ProductsGrid!.Refresh();
                await Toast!.ShowSuccessToast("Product deleted Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "Active")
        {
            try
            {
                foreach (var item in ProductsGrid!.SelectedRecords)
                {
                    Products!.Find(p => p.Id == item.Id)!.Active = !item.Active;
                }

                var productsToActivate = ProductsGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Product_ActivateProductAsync(productsToActivate);
                await ProductsGrid!.Refresh();
                await Toast!.ShowSuccessToast("Product updated Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
            }
        }
    }
}
