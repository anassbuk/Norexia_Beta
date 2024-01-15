using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Models.Sale;

namespace NorexiaGestionCommercialeWebUI.Pages.Delivery.Deliverer;
public partial class EditDelivererForm
{
    [Parameter]
    public Guid Id { get; set; }
    [Inject]
    NavigationManager? Navigation { get; set; }
    public DelivererDto Deliverer { get; set; } = new();
    private EditContext? EC { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    ToastComponent? Toast;

    protected override void OnInitialized()
    {
        EC = new EditContext(Deliverer);

        if (AppStates!.Deliverer is null)
            Navigation!.NavigateTo("/Deliverers");

        Deliverer = AppStates!.Deliverer!;
        EC = new EditContext(Deliverer);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var command = Mapper!.Map<UpdateDelivererCommand>(Deliverer);
                await GCApiProxy!.Proxy.Deliverer_UpdateDelivererAsync(Id, command);
                await Toast!.ShowSuccessToast("Deliverer saved Successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
