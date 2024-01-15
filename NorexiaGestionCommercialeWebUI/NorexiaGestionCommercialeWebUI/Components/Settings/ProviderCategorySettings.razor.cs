using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class ProviderCategorySettings
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<ProviderCategoryDto>? ProviderCategories { get; set; }

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
        ProviderCategories = (List<ProviderCategoryDto>)await GCApiProxy!.Proxy.ProviderCategory_GetProviderCategoriesAsync();
    }

    public async Task OnCategoryActionBegin(ActionEventArgs<ProviderCategoryDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (ProviderCategories!.Any(f => f.Name == Args.Data.Name))
            {
                DialogValidationMessage = $"La catégorie avec avec le nom {Args.Data.Name} existe déjà";
                Args.Cancel = true;
                return;
            }

            if (Args.Action == "Add")
            {
                try
                {
                    var Command = new CreateProviderCategoryCommand()
                    {
                        Name = Args.Data.Name,
                    };
                    Guid addedId = await GCApiProxy!.Proxy.ProviderCategory_CreateProviderCategoryAsync(Command);
                    Args.Data.Id = addedId;
                    await ShowSuccessToast("Category added Successfully");
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
                    var Command = new UpdateProviderCategoryCommand()
                    {
                        Id = Args.Data.Id,
                        Name = Args.Data.Name,
                    };
                    await GCApiProxy!.Proxy.ProviderCategory_UpdateProviderCategoryAsync(Args.Data.Id, Command);

                    await ShowSuccessToast("Category Edited Successfully");
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
                await GCApiProxy!.Proxy.ProviderCategory_DeleteProviderCategoryAsync(toDelete);

                await ShowSuccessToast("Category deleted Successfully");
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
