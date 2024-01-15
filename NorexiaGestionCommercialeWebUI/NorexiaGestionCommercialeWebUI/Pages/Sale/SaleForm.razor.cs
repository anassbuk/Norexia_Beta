using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Sale;
public partial class SaleForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public SaleCommand SaleCommand { get; set; } = new();
    public OwnedPaymentTerms? DefaultPaymentTerms { get; set; }
    private EditContext? EC { get; set; }
    private List<PriceGroupDto>? PriceGroups;
    public List<ProductAvailabilityDto> ProductAvailablities { get; set; } = new();
    private Guid? DefaultPriceGroupId;

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<PaymentMeanDto>? PaymentMeans { get; set; }


    ToastComponent? Toast;
    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(SaleCommand);
        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();
        ProductAvailablities = (List<ProductAvailabilityDto>)await GCApiProxy!.Proxy.ProductAvailability_GetProductAvailabilitiesAsync();
        DefaultPaymentTerms = Mapper!.Map<OwnedPaymentTerms>(await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync());
        PaymentMeans = (List<PaymentMeanDto>)await GCApiProxy!.Proxy.PaymentMean_GetPaymentMeansAsync();
    }
    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreateSaleOrderCommand>(SaleCommand);
                await GCApiProxy!.Proxy.SaleOrder_CreateSaleOrderAsync(createCommand);
                await Toast!.ShowSuccessToast("Sale order added Successfully");
                Navigation!.NavigateTo("/Sales");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
