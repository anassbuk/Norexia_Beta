using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class ProductAvailabilitySettings
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<ProductAvailabilityDto>? ProductAvailabilities { get; set; }
    private string DialogValidationMessage = string.Empty;

    [Parameter]
    public EventCallback<string> OnShowSuccessToast { get; set; }

    protected async Task ShowSuccessToast(string content)
    {
        await OnShowSuccessToast.InvokeAsync(content);
    }

    [Parameter]
    public EventCallback<string> OnShowErrorToast { get; set; }

    protected async Task ShowErrorToast(string content)
    {
        await OnShowErrorToast.InvokeAsync(content);
    }

    protected override async Task OnInitializedAsync()
    {
        ProductAvailabilities = (List<ProductAvailabilityDto>)await GCApiProxy!.Proxy.ProductAvailability_GetProductAvailabilitiesAsync();
    }

    public async Task OnAvailabilityActionBegin(ActionEventArgs<ProductAvailabilityDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (ProductAvailabilities!.Any(f => f.Name == Args.Data.Name))
            {
                DialogValidationMessage = $"Le canal de vente avec le nom {Args.Data.Name} existe déjà";
                Args.Cancel = true;
                return;
            }
            if (Args.Action == "Add")
            {
                try
                {
                    var createProductAvailabilityCommand = new CreateProductAvailabilityCommand()
                    {
                        Name = Args.Data.Name,
                    };
                    Guid addedId = await GCApiProxy!.Proxy.ProductAvailability_CreateProductAvailabilityAsync(createProductAvailabilityCommand);
                    Args.Data.Id = addedId;
                    await ShowSuccessToast("Product availability added Successfully");
                }
                catch (Exception ex)
                {
                    await ShowErrorToast(ex.Message);
                    Args.Cancel = true;
                }
            }
            else
            {
                try
                {
                    var updateProductAvailabilityCommand = new UpdateProductAvailabilityCommand()
                    {
                        Id = Args.Data.Id,
                        Name = Args.Data.Name,
                    };
                    await GCApiProxy!.Proxy.ProductAvailability_UpdateProductAvailabilityAsync(Args.Data.Id, updateProductAvailabilityCommand);

                    await ShowSuccessToast("Product availability Edited Successfully");
                }
                catch (Exception ex)
                {
                    await ShowErrorToast(ex.Message);
                    Args.Cancel = true;
                }
            }
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                var toDelete = new List<Guid>() { Args.Data.Id };
                await GCApiProxy!.Proxy.ProductAvailability_DeleteProductAvailabilityAsync(toDelete);

                await ShowSuccessToast("Product availability deleted Successfully");
            }
            catch (Exception ex)
            {
                await ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }

        DialogValidationMessage = string.Empty;
    }
}
