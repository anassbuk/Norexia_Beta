using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Client;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Client;
public partial class EditClientForm
{
    [Parameter]
    public Guid Id { get; set; }
    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    public ClientCommand ClientCommand { get; set; } = new();
    private EditContext? EC { get; set; }
    private List<CustomerCategoryDto>? CustomerCategories;

    ToastComponent? Toast;

    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(ClientCommand);
        CustomerCategories = (List<CustomerCategoryDto>)await GCApiProxy!.Proxy.CustomerCategory_GetCustomerCategoriesAsync();
        var client = await GCApiProxy.Proxy.Customer_GetCustomerAsync(Id);
        ClientCommand = Mapper!.Map<ClientCommand>(client);
        EC = new EditContext(ClientCommand);
    }

    public async Task SaveClient()
    {
        try
        {
            if (EC!.Validate())
            {
                var command = Mapper!.Map<UpdateCustomer>(ClientCommand);
                await GCApiProxy!.Proxy.Customer_UpdateCustomerAsync(Id, command);
                await Toast!.ShowSuccessToast("Client saved Successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
