using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Components;
using NorexiaGestionCommercialeWebUI.Models.Provider;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Pages.Delivery.Deliverer;
public partial class DelivererForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public DelivererDto Deliverer { get; set; } = new();
    private EditContext? EC { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    ToastComponent? Toast;

    protected override void OnInitialized()
    {
        EC = new EditContext(Deliverer);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreateDelivererCommand>(Deliverer);
                await GCApiProxy!.Proxy.Deliverer_CreateDelivererAsync(createCommand);
                await Toast!.ShowSuccessToast("Deliverer added Successfully");
                Navigation!.NavigateTo("/Deliverers");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
