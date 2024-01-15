using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Components.CreditNote;
using NorexiaGestionCommercialeWebUI.Models.CreditNote;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Pages.CreditNote
{
    public partial class CreditNoteForm
    {
        [Inject]
        NavigationManager? Navigation { get; set; }
        public CreditNoteCommand CreditNoteCommand { get; set; } = new();
        public EditContext? EC { get; set; }
        private List<PriceGroupDto>? PriceGroups;
        private Guid? DefaultPriceGroupId;
        public OwnedPaymentTerms? DefaultPaymentTerms { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        ToastComponent? Toast;

       
        protected async override Task OnInitializedAsync()
        {
            EC = new EditContext(CreditNoteCommand);
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
                    var createCommand = Mapper!.Map<CreateCreditNoteCommand>(CreditNoteCommand);
                    await GCApiProxy!.Proxy.CreditNote_CreateCreditNoteAsync(createCommand);
                    await Toast!.ShowSuccessToast("CreditNote added Successfully");
                    Navigation!.NavigateTo("/CreditNotes");
                }
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
            }
        }
       
        

    }
}
