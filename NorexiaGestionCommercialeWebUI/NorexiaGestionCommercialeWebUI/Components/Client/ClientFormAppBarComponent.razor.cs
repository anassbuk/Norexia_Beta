using Microsoft.AspNetCore.Components;

namespace NorexiaGestionCommercialeWebUI.Components.Client;
public partial class ClientFormAppBarComponent
{
    [Parameter]
    public EventCallback OnSaveClientClicked { get; set; }

    protected async Task SaveClientClicked()
    {
        await OnSaveClientClicked.InvokeAsync();
    }
}
