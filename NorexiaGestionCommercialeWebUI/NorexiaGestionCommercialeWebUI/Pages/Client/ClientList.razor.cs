using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;

namespace NorexiaGestionCommercialeWebUI.Pages.Client;
public partial class ClientList
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<CustomerDto>? Customers { get; set; }

    public SfGrid<CustomerDto>? CustomersGrid { get; set; }

    private readonly List<object> Toolbaritems = new() { "Add", "Edit", "Delete", new ItemModel() { Text = "Activer / Désactiver", TooltipText = "", PrefixIcon = "e-circle-check", Id = "Active" } };

    ToastComponent? Toast;
    protected async override Task OnInitializedAsync()
    {
        Customers = (List<CustomerDto>)await GCApiProxy!.Proxy.Customer_GetCustomersAsync();
    }

    public async Task OnActionBegin(ActionEventArgs<CustomerDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo("/Clients/New");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            Args.Cancel = true;
            Navigation!.NavigateTo($"/Clients/{Args.Data.Id}");
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            Args.Cancel = true;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                foreach (var item in CustomersGrid!.SelectedRecords)
                {
                    Customers!.Remove(item);
                }
                var toDelete = CustomersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Customer_DeleteCustomerAsync(toDelete);
                await CustomersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Client deleted Successfully");
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
                foreach (var item in CustomersGrid!.SelectedRecords)
                {
                    Customers!.Find(p => p.Id == item.Id)!.Active = !item.Active;
                }

                var toActivate = CustomersGrid!.SelectedRecords.Select(r => r.Id).ToList();
                await GCApiProxy!.Proxy.Customer_ActivateCustomerAsync(toActivate);
                await CustomersGrid!.Refresh();
                await Toast!.ShowSuccessToast("Client updated Successfully");
            }
            catch (Exception ex)
            {
                await Toast!.ShowErrorToast(ex.Message);
            }
        }
    }
}
