using Microsoft.AspNetCore.Components;

namespace NorexiaGestionCommercialeWebUI.Components.Provider;
public partial class ProviderFormAppBarComponent
{
    [Parameter]
    public EventCallback OnSaveProviderClicked { get; set; }

    protected async Task SaveProviderClicked()
    {
        await OnSaveProviderClicked.InvokeAsync();
    }
}
