using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Notifications;

namespace NorexiaGestionCommercialeWebUI.Pages.Provider;
public partial class ProviderList
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<ProvidersDto>? Providers { get; set; }

    public SfGrid<ProvidersDto>? ProvidersGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete", new ItemModel() { Text = "Activer / Désactiver", TooltipText = "", PrefixIcon = "e-circle-check", Id = "Active" } };

    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        Providers = (List<ProvidersDto>)await GCApiProxy!.Proxy.Provider_GetProvidersAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<ProvidersDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Providers/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo($"/Providers/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                foreach (var item in ProvidersGrid!.SelectedRecords)
                {
                    Providers!.Remove(item);
                }
                var toDelete = ProvidersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Provider_DeleteProviderAsync(toDelete);
                await ProvidersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Provider deleted Successfully");
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
                foreach (var item in ProvidersGrid!.SelectedRecords)
                {
                    Providers!.Find(p => p.Id == item.Id)!.Active = !item.Active;
                }

                var toActivate = ProvidersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Provider_ActivateProviderAsync(toActivate);
                await ProvidersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Provider updated Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
            }
        }
    }
}
