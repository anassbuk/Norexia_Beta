using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Models.Quotation;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Pages.Quotation;

    public partial class EditQuotationForm
    {
    [Parameter]
    public Guid Id { get; set; }
    public QuotationCommand QuotationCommand { get; set; } = new();
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

    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(QuotationCommand);

        if (AppStates!.Quotation is null)
            Navigation!.NavigateTo("/Quotations");

        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();

        QuotationCommand = Mapper!.Map<QuotationCommand>(AppStates!.Quotation);

        if (QuotationCommand!.CustomerId != null)
             Customer = await GCApiProxy!.Proxy.Customer_GetCustomerAsync((Guid)QuotationCommand.CustomerId);
        
            ProductAvailablities = (List<ProductAvailabilityDto>) await GCApiProxy!.Proxy.ProductAvailability_GetProductAvailabilitiesAsync();
            DefaultPaymentTerms = Mapper!.Map<OwnedPaymentTerms>(await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync());  

             var lines = await GCApiProxy!.Proxy.Quotation_GetQutationLinesAsync(Id);
             QuotationCommand!.QuotationLines = lines;
              EC = new EditContext(QuotationCommand);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var command = Mapper!.Map<UpdateQuotationCommand>(QuotationCommand);
                await GCApiProxy!.Proxy.Quotation_UpdateQoutationAsync(Id,command);
                await Toast!.ShowSuccessToast("Quotation updated successfully");
               
                
            }

        }
        catch (Exception)
        {
            await Toast!.ShowErrorToast("Error");
        }
    }


}

