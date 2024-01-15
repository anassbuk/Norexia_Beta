using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class PriceGroupSettings
{

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<PriceGroupDto>? PriceGroupList { get; set; }

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
        PriceGroupList = (List<PriceGroupDto>)await GCApiProxy!.Proxy.PriceGroup_GetPriceGroupsAsync();
    }

    public async Task OnPriceGroupActionBegin(ActionEventArgs<PriceGroupDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (PriceGroupList!.Any(f => f.Name == Args.Data.Name))
            {
                DialogValidationMessage = $"Le groupe de prix avec le nom {Args.Data.Name} existe déjà";
                Args.Cancel = true;
                return;
            }

            if (Args.Action == "Add")
            {
                try
                {
                    var createPriceGroupCommand = new CreatePriceGroupCommand()
                    {
                        Name = Args.Data.Name,
                    };
                    Guid addedGroupId = await GCApiProxy!.Proxy.PriceGroup_CreatePriceGroupAsync(createPriceGroupCommand);
                    Args.Data.Id = addedGroupId;
                    await ShowSuccessToast("Price group Added Successfully");
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
                    var updatePriceGroupCommand = new UpdatePriceGroupCommand()
                    {
                        Id = Args.Data.Id,
                        Name = Args.Data.Name,
                    };
                    await GCApiProxy!.Proxy.PriceGroup_UpdatePriceGroupAsync(Args.Data.Id, updatePriceGroupCommand);

                    await ShowSuccessToast("Price group Edited Successfully");
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
                var priceGroupsToDelete = new List<Guid>() { Args.Data.Id };
                await GCApiProxy!.Proxy.PriceGroup_DeletePriceGroupAsync(priceGroupsToDelete);

                await ShowSuccessToast("Price group deleted Successfully");
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
