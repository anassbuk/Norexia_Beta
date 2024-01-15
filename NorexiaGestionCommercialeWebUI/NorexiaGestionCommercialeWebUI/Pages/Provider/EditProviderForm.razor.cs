using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Provider;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Provider;
public partial class EditProviderForm
{
    [Parameter]
    public Guid Id { get; set; }
    public ProviderCommand ProviderCommand { get; set; } = new();
    private EditContext? EC { get; set; }
    private List<ProviderCategoryDto>? ProviderCategories;

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    ToastComponent? Toast;

    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(ProviderCommand);
        ProviderCategories = (List<ProviderCategoryDto>)await GCApiProxy!.Proxy.ProviderCategory_GetProviderCategoriesAsync();
        var provider = await GCApiProxy.Proxy.Provider_GetProviderAsync(Id);
        ProviderCommand = Mapper!.Map<ProviderCommand>(provider);
        EC = new EditContext(ProviderCommand);
    }

    public async Task SaveClient()
    {
        try
        {
            if (EC!.Validate())
            {
                var command = Mapper!.Map<UpdateProvider>(ProviderCommand);
                await GCApiProxy!.Proxy.Provider_UpdateProviderAsync(Id, command);
                await Toast!.ShowSuccessToast("Provider saved Successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
