using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.AppState;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Notifications;
using NorexiaGestionCommercialeWebUI.Models.Purchase;
using NorexiaGestionCommercialeWebUI.Components;

namespace NorexiaGestionCommercialeWebUI.Pages.Purchase;
public partial class EditPurchaseForm
{
    [Parameter]
    public Guid Id { get; set; }
    public PurchaseCommand? PurchaseCommand = new();
    public ProviderDetailsDto? Provider;
    private EditContext? EC { get; set; }

    [Inject]
    public IMapper? Mapper { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Inject]
    public States? AppStates { get; set; }
    [Inject]
    NavigationManager? Navigation { get; set; }

    ToastComponent? Toast;
    protected override async Task OnInitializedAsync()
    {
        EC = new EditContext(PurchaseCommand!);

        if (AppStates!.PurchaseOrder is null)
            Navigation!.NavigateTo("/Purchases");

        PurchaseCommand = Mapper!.Map<PurchaseCommand>(AppStates!.PurchaseOrder);

        if (PurchaseCommand!.ProviderId != null)
            Provider = await GCApiProxy!.Proxy.Provider_GetProviderAsync((Guid)PurchaseCommand.ProviderId);

        var lines = await GCApiProxy!.Proxy.PurchaseOrder_GetPurchaseOrderLinesAsync(Id);
        PurchaseCommand!.PurchaseOrderLines = lines;

        EC = new EditContext(PurchaseCommand);
    }

    public async Task Save()
    {
        try
        {
            if (EC!.Validate())
            {
                var command = Mapper!.Map<UpdatePurchaseOrderCommand>(PurchaseCommand);
                await GCApiProxy!.Proxy.PurchaseOrder_UpdatePurchaseOrderAsync(Id, command);
                await Toast!.ShowSuccessToast("Purchase order updated successfully");
            }
        }
        catch (Exception ex)
        {
            await Toast!.ShowErrorToast(ex.Message);
        }
    }
}
