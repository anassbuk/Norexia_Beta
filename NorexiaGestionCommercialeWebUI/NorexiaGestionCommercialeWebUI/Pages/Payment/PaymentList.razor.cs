using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Pages.Payment;
public partial class PaymentList
{
    [Inject]
    NavigationManager? Navigation { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    public List<PaymentDto>? Payments { get; set; }

    public SfGrid<PaymentDto>? PaymentsGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete" };
    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        Payments = (List<PaymentDto>)await GCApiProxy!.Proxy.Payment_GetPaymentsAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<PaymentDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Payments/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            AppStates!.Payment = Args.Data;
            Navigation!.NavigateTo($"/Payments/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                var toDelete = PaymentsGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Payment_DeletePaymentAsync(toDelete);
                foreach (var item in PaymentsGrid!.SelectedRecords)
                {
                    Payments!.Remove(item);
                }
                await PaymentsGrid!.Refresh();
                await Toast!.ShowSuccessToast("Payment deleted Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }
}
