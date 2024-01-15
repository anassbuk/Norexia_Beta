using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Client;
using Syncfusion.Blazor.Diagrams;

namespace NorexiaGestionCommercialeWebUI.Components.Client;
public partial class ClientGeneralInformationComponent
{
    [Parameter]
    public ClientCommand? ClientCommand { get; set; }

    [Parameter]
    public EventCallback<ClientCommand> ClientCommandChanged { get; set; }

    [Parameter]
    public List<CustomerCategoryDto>? CustomerCategories { get; set; }

    protected override void OnInitialized()
    {
        if(ClientCommand!.ClientType is null)
            ClientCommand!.ClientType = ClientType.Particular;
    }
    private async Task OnClientTypeChange(ChangeEventArgs args)
    {
        await ClientCommandChanged.InvokeAsync(ClientCommand);
    }
}
