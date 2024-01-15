using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Models.Quotation;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Pages.Quotation;

    public partial class QuotationForm
    {
        [Inject]
        NavigationManager? Navigation { get; set; }
        public QuotationCommand QuotationCommand { get; set; } = new();
        public OwnedPaymentTerms? DefaultPaymentTerms { get; set; }
        private EditContext? EC { get; set; }
        private List<PriceGroupDto>? PriceGroups = new();
        private Guid? DefaultPriceGroupId;

        [Inject]
        public IMapper? Mapper { get; set; }
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }

        ToastComponent? Toast;

        protected override async Task OnInitializedAsync()
        {
            EC = new EditContext(QuotationCommand);
            PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
            DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();
            DefaultPaymentTerms = Mapper!.Map<OwnedPaymentTerms>(await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync());
        }

        public async Task Save()
        {
            try
            {
                if (EC!.Validate())
                {
                    var createCommand = Mapper!.Map<CreateQuotationCommand>(QuotationCommand);
                    await GCApiProxy!.Proxy.Quotation_CreateQuotationAsync(createCommand);
                    await Toast!.ShowSuccessToast("Quotation added Successfully");
                    Navigation!.NavigateTo("/Quotations");
                }

                var message = string.Join(Environment.NewLine, EC!.GetValidationMessages());
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
            }
        }   


    }

