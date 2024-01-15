using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Provider;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Provider;
public partial class ProviderForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
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
    }

    public async Task SaveProvider()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreateProvider>(ProviderCommand);
                await GCApiProxy!.Proxy.Provider_CreateProviderAsync(createCommand);
                await Toast!.ShowSuccessToast("Provider added Successfully");
                Navigation!.NavigateTo("/Providers");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
