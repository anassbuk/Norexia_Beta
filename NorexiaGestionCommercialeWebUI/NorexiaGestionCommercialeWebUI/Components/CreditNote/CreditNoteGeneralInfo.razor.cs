using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.CreditNote;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.CreditNote
{
    public partial class CreditNoteGeneralInfo
    {
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        [Parameter]
        public CreditNoteCommand? CreditNoteCommand { get; set; }
        [Parameter]
        public EventCallback<CreditNoteCommand> CreditNoteCommandChanged { get; set; }

        [Parameter]
       public CustomerDetailsDto? Customer { get; set; }
        [Parameter]
        public EventCallback<CustomerDetailsDto> CustomerChanged { get; set; }
        [Parameter]
        public EditContext? EC { get; set; }
        private string? InvoiceSearchTerm;
        private bool IsDialogVisible;
        private bool IsSaleLinesDialogVisible;
        private string DialogMessage = string.Empty;


        private List<CreditNoteLineDto> InvoiceLines = new() ;
        private SfGrid<CreditNoteLineDto>? InvoiceLinesGrid ;
        


        protected override void OnParametersSet()
        {
            
            InvoiceSearchTerm = CreditNoteCommand!.InvoiceRef;
           
        }
            
       
        
        public async Task SearchInvoice(MouseEventArgs args)
        {
            if (CreditNoteCommand!.CreditOrigin == CreditOrigin.Invoice)
            {
                

                try
                {
                    if (!string.IsNullOrWhiteSpace(InvoiceSearchTerm))
                    {
                        var invoice = await GCApiProxy!.Proxy.Invoice_GetInvoiceAsCreditNoteAsync(InvoiceSearchTerm);

                        CreditNoteCommand!.InvoiceId = invoice.Id;
                        CreditNoteCommand!.InvoiceRef = invoice.Reference;
                        CreditNoteCommand.CustomerId = invoice.CustomerId;
                        CreditNoteCommand!.CustomerRef = invoice.CustomerRef;

                        InvoiceLines = invoice.CreditNoteLines.ToList();
                        IsSaleLinesDialogVisible = true;
                       
                    }
                }catch (Exception)
                {
                    DialogMessage = $"Facture avec le terme de recherche '{InvoiceSearchTerm}' introuvable";
                    IsDialogVisible = true;
                }
            }
        }


        private void DialogOkClick()
        {
            this.IsDialogVisible = false;
        }

        private async Task SaleLinesDialogOkClick()
        {
            CreditNoteCommand!.CreditNoteLines = await InvoiceLinesGrid!.GetSelectedRecordsAsync();
            IsSaleLinesDialogVisible = false;
            await CreditNoteCommandChanged.InvokeAsync(CreditNoteCommand);
        }

       

        readonly List<DropDownCreditOrigin> ddCreditOrigins = new()
        {
            new DropDownCreditOrigin() { DisplayName = "Facture", CreditOrigin = CreditOrigin.Invoice},
            
        };

        readonly List<DropDownCreditAction> ddCreditActions = new()
        {
            new DropDownCreditAction() { DisplayName = "Remboursement", CreditAction = CreditAction.Refund},
            new DropDownCreditAction() { DisplayName = "Déduction Facture", CreditAction = CreditAction.InvoiceDeduction},
            new DropDownCreditAction() { DisplayName = "Bon d’achat", CreditAction = CreditAction.GoodBuy},

        };  
        public class DropDownCreditOrigin
        {
            public string? DisplayName { get; set; }
            public CreditOrigin CreditOrigin { get; set; }
        }

        public class DropDownCreditAction
        {
            public string? DisplayName { get; set; }
            public CreditAction CreditAction { get; set; }
        }       






    }
}
