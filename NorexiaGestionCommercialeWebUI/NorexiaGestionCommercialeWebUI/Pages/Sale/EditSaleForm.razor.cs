using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Sale;
public partial class EditSaleForm
{
    [Parameter]
    public Guid Id { get; set; }
    public SaleCommand SaleCommand { get; set; } = new();
    private EditContext? EC { get; set; }
    private List<PriceGroupDto>? PriceGroups;
    private Guid? DefaultPriceGroupId;
    private CustomerDetailsDto? Customer;
    public List<ProductAvailabilityDto> ProductAvailablities { get; set; } = new();

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    [Inject]
    NavigationManager? Navigation { get; set; }

    ToastComponent? Toast;
    public OwnedPaymentTerms? DefaultPaymentTerms { get; set; }
    public List<PaymentMeanDto>? PaymentMeans { get; set; }
    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(SaleCommand);

        if (AppStates!.SaleOrder is null)
            Navigation!.NavigateTo("/Sales");

        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();

        SaleCommand = Mapper!.Map<SaleCommand>(AppStates!.SaleOrder);

        if (SaleCommand!.CustomerId != null)
            Customer = await GCApiProxy!.Proxy.Customer_GetCustomerAsync((Guid)SaleCommand.CustomerId);

        ProductAvailablities = (List<ProductAvailabilityDto>)await GCApiProxy!.Proxy.ProductAvailability_GetProductAvailabilitiesAsync();
        DefaultPaymentTerms = Mapper!.Map<OwnedPaymentTerms>(await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync());
        PaymentMeans = (List<PaymentMeanDto>)await GCApiProxy!.Proxy.PaymentMean_GetPaymentMeansAsync();

        var lines = await GCApiProxy!.Proxy.SaleOrder_GetSaleOrderLinesAsync(Id);
        SaleCommand!.SaleOrderLines = lines;

        var payments = await GCApiProxy!.Proxy.Payment_GetSaleOrderPaymentsAsync(Id);
        SaleCommand!.SalePayments = Mapper!.Map<List<PaymentDto>>(payments);

        EC = new EditContext(SaleCommand);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var command = Mapper!.Map<UpdateSaleOrderCommand>(SaleCommand);
                await GCApiProxy!.Proxy.SaleOrder_UpdateSaleOrderAsync(Id, command);
                await Toast!.ShowSuccessToast("Sale order updated successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
