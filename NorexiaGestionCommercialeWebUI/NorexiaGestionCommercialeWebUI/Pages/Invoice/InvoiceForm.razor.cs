using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Navigations;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using NorexiaGestionCommercialeWebUI.Components.Invoice;

namespace NorexiaGestionCommercialeWebUI.Pages.Invoice;
public partial class InvoiceForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public InvoiceCommand InvoiceCommand { get; set; } = new();
    public EditContext? EC { get; set; }
    private List<PriceGroupDto>? PriceGroups;
    private Guid? DefaultPriceGroupId;
    public OwnedPaymentTerms? DefaultPaymentTerms { get; set; }
    public List<CurrencyDto>? Currencies { get; set; }

    private InvoiceLinesComponent? InvoiceLinesComponent;

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    ToastComponent? Toast;
    public List<PaymentMeanDto>? PaymentMeans { get; set; }
    protected async override Task OnInitializedAsync()
    {
        EC = new EditContext(InvoiceCommand);
        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();
        Currencies = (List<CurrencyDto>)await GCApiProxy!.Proxy.Currency_GetCurrenciesAsync();
        DefaultPaymentTerms = Mapper!.Map<OwnedPaymentTerms>(await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync());
        PaymentMeans = (List<PaymentMeanDto>)await GCApiProxy!.Proxy.PaymentMean_GetPaymentMeansAsync();
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreateInvoiceCommand>(InvoiceCommand);
                await GCApiProxy!.Proxy.Invoice_CreateInvoiceAsync(createCommand);
                await Toast!.ShowSuccessToast("Invoice added Successfully");
                Navigation!.NavigateTo("/Invoices");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }

    private void FillDepositInvoiceData()
    {
        InvoiceLinesComponent!.FillDepositInvoiceData();
    }
}
