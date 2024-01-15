using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components.Invoice;

namespace NorexiaGestionCommercialeWebUI.Pages.Invoice;
public partial class EditInvoiceForm
{
    [Parameter]
    public Guid Id { get; set; }
    [Inject]
    NavigationManager? Navigation { get; set; }
    public InvoiceCommand InvoiceCommand { get; set; } = new();
    public OwnedPaymentTerms? DefaultPaymentTerms { get; set; }
    public List<CurrencyDto>? Currencies { get; set; }
    public EditContext? EC { get; set; }
    private List<PriceGroupDto>? PriceGroups;
    private Guid? DefaultPriceGroupId;
    private InvoiceLinesComponent? InvoiceLinesComponent;
    [Inject]
    public States? AppStates { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    ToastComponent? Toast;
    public List<PaymentMeanDto>? PaymentMeans { get; set; }
    protected async override Task OnInitializedAsync()
    {
        EC = new EditContext(InvoiceCommand);

        if (AppStates!.Invoice is null)
            Navigation!.NavigateTo("/Invoices");

        DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();
        PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
        Currencies = (List<CurrencyDto>)await GCApiProxy!.Proxy.Currency_GetCurrenciesAsync();
        DefaultPaymentTerms = Mapper!.Map<OwnedPaymentTerms>(await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync());
        PaymentMeans = (List<PaymentMeanDto>)await GCApiProxy!.Proxy.PaymentMean_GetPaymentMeansAsync();

        InvoiceCommand = Mapper!.Map<InvoiceCommand>(AppStates!.Invoice);

        var lines = await GCApiProxy!.Proxy.Invoice_GetInvoiceLinesAsync(Id);
        InvoiceCommand!.InvoiceLines = lines;


        var payments = await GCApiProxy!.Proxy.Payment_GetInvoicePaymentsAsync(Id);
        InvoiceCommand!.InvoicePayments = Mapper!.Map<List<PaymentDto>>(payments);

        EC = new EditContext(InvoiceCommand);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var updateCommand = Mapper!.Map<UpdateInvoiceCommand>(InvoiceCommand);
                await GCApiProxy!.Proxy.Invoice_UpdateInvoiceAsync(Id, updateCommand);
                await Toast!.ShowSuccessToast("Invoice edited Successfully");
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
