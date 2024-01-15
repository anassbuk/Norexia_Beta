using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Pages.Quotation;

    public partial class QuotationList
    {
        [Inject]
        NavigationManager? Navigation { get; set; }
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        [Inject]
        public States? AppStates { get; set; }
        public List<QuotationDto>? Quotations { get; set; }
        public SfGrid<QuotationDto>? QuotationsGrid { get; set; }
        private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };
        ToastComponent? Toast;


        protected override async Task OnInitializedAsync()
        {
           Quotations = (List<QuotationDto>)await GCApiProxy!.Proxy.Quotation_GetQuotationAsync();
        }

    public async Task OnActionBegin(ActionEventArgs<QuotationDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Quotations/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            AppStates!.Quotation = Args.Data;
            Navigation!.NavigateTo($"/Quotations/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                foreach (var item in QuotationsGrid!.SelectedRecords)
                {
                    Quotations!.Remove(item);
                }
                var toDelete = QuotationsGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Quotation_DeleteQuotationAsync(toDelete);
                await QuotationsGrid!.Refresh();
                await Toast!.ShowSuccessToast("Quotation has been successfully deleted.");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }
}


