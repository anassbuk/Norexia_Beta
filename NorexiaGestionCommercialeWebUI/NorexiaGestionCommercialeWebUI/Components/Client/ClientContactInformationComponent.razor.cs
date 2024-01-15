using Microsoft.AspNetCore.Components;
using NorexiaGestionCommercialeWebUI.Models.Client;

namespace NorexiaGestionCommercialeWebUI.Components.Client;
public partial class ClientContactInformationComponent
{
    [Parameter]
    public ClientCommand? ClientCommand { get; set; }

    [Parameter]
    public EventCallback<ClientCommand> ClientCommandChanged { get; set; }
}
