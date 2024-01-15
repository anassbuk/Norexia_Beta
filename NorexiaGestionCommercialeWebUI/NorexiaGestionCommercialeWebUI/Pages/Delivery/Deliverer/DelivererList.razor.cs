using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace NorexiaGestionCommercialeWebUI.Pages.Delivery.Deliverer;
public partial class DelivererList
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<DelivererDto>? Deliverers { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    public SfGrid<DelivererDto>? DeliverersGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete", new ItemModel() { Text = "Activer / Désactiver", TooltipText = "", PrefixIcon = "e-circle-check", Id = "Active" } };

    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        Deliverers = (List<DelivererDto>)await GCApiProxy!.Proxy.Deliverer_GetDeliverersAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<DelivererDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Deliverers/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            AppStates!.Deliverer = Args.Data;
            Navigation!.NavigateTo($"/Deliverers/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                var toDelete = DeliverersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Deliverer_DeleteDelivererAsync(toDelete);


                foreach (var item in DeliverersGrid!.SelectedRecords)
                {
                    Deliverers!.Remove(item);
                }
                await DeliverersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Deliverers deleted Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "Active")
        {
            try
            {
                var toActivate = DeliverersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Deliverer_ActivateDelivererAsync(toActivate);

                foreach (var item in DeliverersGrid!.SelectedRecords)
                {
                    Deliverers!.Find(p => p.Id == item.Id)!.Active = !item.Active;
                }

                await DeliverersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Deliverers updated Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
            }
        }
    }
}
