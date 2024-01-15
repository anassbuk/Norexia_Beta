using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Components.Product;
using NorexiaGestionCommercialeWebUI.Models.Product;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Notifications;

namespace NorexiaGestionCommercialeWebUI.Pages.Product;
public partial class ProductForm
{
    public ProductCommand ProductCommand { get; set; } = new();
    [Inject]
    NavigationManager? Navigation { get; set; }

    private ProductIdentificationComponent? productIdentificationComponent;
    private ProductClassificationComponent? productClassificationComponent;
    private ProductUnitComponent? productUnitComponent;
    private ProductSalesComponent? productSalesComponent;
    public List<UnitDto>? Units { get; set; } = new();
    public List<FamilyDto>? Families { get; set; } = new();
    public List<ClassDto>? Classes { get; set; } = new();
    public List<ProductAvailabilityDto> ProductAvailablities { get; set; } = new();
    public Guid? DefaultPriceGroupId { get; set; } = new();
    public List<PriceGroupDto>? PriceGroups { get; set; } = new();

    private EditContext? EC { get; set; }
    private SfTab? SfTab;

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    ToastComponent? Toast;

    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(ProductCommand);
        Families = (List<FamilyDto>)await GCApiProxy!.Proxy.Family_GetFamiliesAsync();
        Classes = (List<ClassDto>)await GCApiProxy!.Proxy.Class_GetClassesAsync();
        ProductAvailablities = (List<ProductAvailabilityDto>)await GCApiProxy!.Proxy.ProductAvailability_GetProductAvailabilitiesAsync();
        Units = (List<UnitDto>)await GCApiProxy!.Proxy.Unit_GetUnitsAsync();
        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();
        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            await SfTab!.SelectAsync(1);
            await SfTab!.SelectAsync(0);
        }
    }

    public async Task SaveProduct()
    {
        try
        {
            await productIdentificationComponent!.HandleProductIdentificationInfo();

            await productClassificationComponent!.HandleProductClassificationInfo();

            bool unitValidate = await productUnitComponent!.HandleProductUnitsInfo();

            bool pricesValidate = await productSalesComponent!.HandleProductPrices();

            if (EC!.Validate() && unitValidate && pricesValidate)
            {
                var createProductCommand = Mapper!.Map<CreateProductCommand>(ProductCommand);
                await GCApiProxy!.Proxy.Product_CreateProductAsync(createProductCommand);
                await Toast!.ShowSuccessToast("Product added Successfully");
                Navigation!.NavigateTo("/Products");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
