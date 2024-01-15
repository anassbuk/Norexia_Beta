using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Navigations;
using Norexia.Core.Facade.Client.Sdk;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Client;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Client;
public partial class ClientForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public ClientCommand ClientCommand { get; set; } = new();
    private EditContext? EC { get; set; }
    private List<CustomerCategoryDto>? CustomerCategories;

    ToastComponent? Toast;

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(ClientCommand);
        CustomerCategories = (List<CustomerCategoryDto>)await GCApiProxy!.Proxy.CustomerCategory_GetCustomerCategoriesAsync();
    }
    public async Task SaveClient()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreateCustomer>(ClientCommand);
                await GCApiProxy!.Proxy.Customer_CreateCustomerAsync(createCommand);
                await Toast!.ShowSuccessToast("Client added Successfully");
                Navigation!.NavigateTo("/Clients");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
