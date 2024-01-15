using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Pages.CreditNote
{
    public partial class CreditNoteList
    {
        [Inject]
        NavigationManager? Navigation { get; set; }
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        [Inject]
        public States? AppStates { get; set; }
        public List<CreditNoteDto>? CreditNotes { get; set; }
        public SfGrid<CreditNoteDto>? CreditNotesGrid { get; set; }
        private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };
        ToastComponent? Toast;

        protected override async Task OnInitializedAsync()
        {
            CreditNotes = (List<CreditNoteDto>)await GCApiProxy!.Proxy.CreditNote_GetCreditNotesAsync();
        }

        public async Task OnActionBegin(ActionEventArgs<CreditNoteDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                Args.Cancel = true;
                Navigation!.NavigateTo("/CreditNotes/New");
            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                Args.Cancel = true;
                AppStates!.CreditNote = Args.Data;
                Navigation!.NavigateTo($"/CreditNotes/{Args.Data.Id}");
            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
            {
                Args.Cancel = true;
            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                try
                {
                    var toDelete = CreditNotesGrid!.SelectedRecords.Select(r => r.Id).ToList();
                    await GCApiProxy!.Proxy.CreditNote_DeleteCreditNotesAsync(toDelete);
                    foreach (var item in CreditNotesGrid!.SelectedRecords)
                    {
                        CreditNotes!.Remove(item);
                    }
                    await CreditNotesGrid!.Refresh();
                    await Toast!.ShowSuccessToast("CreditNote deleted Successfully");
                }
                catch (Exception ex)
                {
                    await Toast!.ShowErrorToast(ex.Message);
                    Args.Cancel = true;
                }
            }
        }
    }
}
