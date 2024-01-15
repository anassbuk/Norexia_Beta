using Microsoft.AspNetCore.Components;
using NorexiaGestionCommercialeWebUI.Models.Provider;

namespace NorexiaGestionCommercialeWebUI.Components.Provider;
public partial class ProviderContactInformationComponent
{
    [Parameter]
    public ProviderCommand? ProviderCommand { get; set; }

    [Parameter]
    public EventCallback<ProviderCommand> ProviderCommandChanged { get; set; }
}
