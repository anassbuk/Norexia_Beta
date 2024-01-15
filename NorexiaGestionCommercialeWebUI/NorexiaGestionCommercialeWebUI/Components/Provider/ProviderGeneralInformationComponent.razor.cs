using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Provider;

namespace NorexiaGestionCommercialeWebUI.Components.Provider;
public partial class ProviderGeneralInformationComponent
{
    [Parameter]
    public ProviderCommand? ProviderCommand { get; set; }

    [Parameter]
    public EventCallback<ProviderCommand> ProviderCommandChanged { get; set; }

    [Parameter]
    public List<ProviderCategoryDto>? ProviderCategories { get; set; }

    protected override void OnInitialized()
    {
        if (ProviderCommand!.ProviderType is null)
            ProviderCommand!.ProviderType = ProviderType.Particular;
    }
    private async Task OnProviderTypeChange(ChangeEventArgs args)
    {
        await ProviderCommandChanged.InvokeAsync(ProviderCommand);
    }
}
