using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components.Product;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Product;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Product;
public partial class EditProductForm
{
    public ProductCommand ProductCommand { get; set; } = new();

    private ProductIdentificationComponent? productIdentificationComponent;
    private ProductClassificationComponent? productClassificationComponent;
    private ProductUnitComponent? productUnitComponent;
    private ProductSalesComponent? productSalesComponent;
    public List<FamilyDto>? Families { get; set; } = new();
    public List<ClassDto>? Classes { get; set; } = new();
    public List<ProductAvailabilityDto> ProductAvailablities { get; set; } = new();
    public List<UnitDto>? Units { get; set; } = new();
    public Guid? DefaultPriceGroupId { get; set; } = new();
    public List<PriceGroupDto>? PriceGroups { get; set; } = new();
    ToastComponent? Toast;

    [Parameter]
    public Guid Id { get; set; }

    [Parameter]
    public string? ShortDesignation { get; set; }

    private EditContext? EC { get; set; }
    private SfTab? SfTab;

    [Inject]
    public IMapper? Mapper { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    
    protected async override Task OnInitializedAsync()
    {
        EC = new EditContext(ProductCommand!);
        var product = await GCApiProxy!.Proxy.Product_GetProductAsync(Id);

        ProductCommand = Mapper!.Map<ProductCommand>(product);

        EC = new(ProductCommand);

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
                var updateProductCommand = Mapper!.Map<UpdateProductCommand>(ProductCommand);
                await GCApiProxy!.Proxy.Product_UpdateProductAsync(updateProductCommand.Id, updateProductCommand);
                await Toast!.ShowSuccessToast("Product saved Successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }

    }
}
