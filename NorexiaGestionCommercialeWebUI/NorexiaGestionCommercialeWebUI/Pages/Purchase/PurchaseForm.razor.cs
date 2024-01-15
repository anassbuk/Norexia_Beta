using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Purchase;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Purchase;
public partial class PurchaseForm
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public PurchaseCommand PurchaseCommand { get; set; } = new();
    private EditContext? EC { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    ToastComponent? Toast;
    protected override void OnInitialized()
    {
        EC = new EditContext(PurchaseCommand);
    }
    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var createCommand = Mapper!.Map<CreatePurchaseOrderCommand>(PurchaseCommand);
                await GCApiProxy!.Proxy.PurchaseOrder_CreatePurchaseOrderAsync(createCommand);
                await Toast!.ShowSuccessToast("Purchase order added Successfully");
                Navigation!.NavigateTo("/Purchases");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
