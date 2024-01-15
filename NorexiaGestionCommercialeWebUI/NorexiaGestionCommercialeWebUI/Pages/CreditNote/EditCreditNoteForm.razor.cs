using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Components.CreditNote;
using NorexiaGestionCommercialeWebUI.Models.CreditNote;
using NorexiaGestionCommercialeWebUI.Proxies;
using System.Runtime.CompilerServices;

namespace NorexiaGestionCommercialeWebUI.Pages.CreditNote
{
    public partial class EditCreditNoteForm
    {
        [Parameter]
        public Guid Id { get; set; }
        [Inject]
        NavigationManager? Navigation { get; set; }
        public CreditNoteCommand CreditNoteCommand { get; set; } = new();
        public OwnedPaymentTerms? DefaultPaymentTerms { get; set; }
        public EditContext? EC { get; set; }
        private List<PriceGroupDto>? PriceGroups;
        private Guid? DefaultPriceGroupId;

        [Inject]
        public States? AppStates { get; set; }

        [Inject]
        public IMapper? Mapper { get; set; }
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        ToastComponent? Toast;

        protected async override Task OnInitializedAsync()
        {
            EC = new EditContext(CreditNoteCommand);
            if(AppStates!.CreditNote is null)
                Navigation!.NavigateTo("/CreditNotes");

            DefaultPriceGroupId = await GCApiProxy!.Proxy.PriceGroup_GetDefaultPriceGroupAsync();
            PriceGroups = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
            DefaultPaymentTerms = Mapper!.Map<OwnedPaymentTerms>(await GCApiProxy!.Proxy.PaymentTerms_GetPaymentTermsAsync());

            CreditNoteCommand = Mapper!.Map<CreditNoteCommand>(AppStates!.CreditNote);

            var lines = await GCApiProxy!.Proxy.CreditNote_GetInvoiceLinesAsync(Id);
            CreditNoteCommand!.CreditNoteLines = lines;

            
          
            
           
            EC = new EditContext(CreditNoteCommand);

        }

        public async Task Save()
        {
            try
            {
                if (EC!.Validate())
                {
                   var updateCommad = Mapper!.Map<UpdateCreditNoteCommand>(CreditNoteCommand);
                    await GCApiProxy!.Proxy.CreditNote_UpdateCreditNotesAsync(Id, updateCommad);
                    await Toast!.ShowSuccessToast("CreditNote updated Successfully");
                }
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
            }
        }
    }
}
