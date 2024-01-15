using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Pages.Invoice
{
    public partial class InvoiceList
    {
        [Inject]
        NavigationManager? Navigation { get; set; }

        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        [Inject]
        public States? AppStates { get; set; }
        public List<InvoiceDto>? Invoices { get; set; }

        public SfGrid<InvoiceDto>? InvoicesGrid { get; set; }

        private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };
        ToastComponent? Toast;
        protected async override Task OnInitializedAsync()
        {
            Invoices = (List<InvoiceDto>)await GCApiProxy!.Proxy.Invoice_GetInvoicesAsync();
        }

        public async Task OnActionBegin(ActionEventArgs<InvoiceDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
            {
                Args.Cancel = true;
                Navigation!.NavigateTo("/Invoices/New");
            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                Args.Cancel = true;
                AppStates!.Invoice = Args.Data;
                Navigation!.NavigateTo($"/Invoices/{Args.Data.Id}");
            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
            {
                Args.Cancel = true;
            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                try
                {
                    var toDelete = InvoicesGrid!.SelectedRecords.Select(r => r.Id).ToList();
                    await GCApiProxy!.Proxy.Invoice_DeleteInvoiceAsync(toDelete);
                    foreach (var item in InvoicesGrid!.SelectedRecords)
                    {
                        Invoices!.Remove(item);
                    }
                    await InvoicesGrid!.Refresh();
                    await Toast!.ShowSuccessToast("Invoice deleted Successfully");
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
