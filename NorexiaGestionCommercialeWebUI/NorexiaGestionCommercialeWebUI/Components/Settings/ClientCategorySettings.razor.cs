using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class ClientCategorySettings
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<CustomerCategoryDto>? CustomerCategories { get; set; }

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
        CustomerCategories = (List<CustomerCategoryDto>)await GCApiProxy!.Proxy.CustomerCategory_GetCustomerCategoriesAsync();
    }

    public async Task OnCategoryActionBegin(ActionEventArgs<CustomerCategoryDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (CustomerCategories!.Any(f => f.Name == Args.Data.Name))
            {
                DialogValidationMessage = $"La catégorie avec avec le nom {Args.Data.Name} existe déjà";
                Args.Cancel = true;
                return;
            }

            if (Args.Action == "Add")
            {
                try
                {
                    var Command = new CreateCustomerCategoryCommand()
                    {
                        Name = Args.Data.Name,
                    };
                    Guid addedId = await GCApiProxy!.Proxy.CustomerCategory_CreateCustomerCategoryAsync(Command);
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
                    var Command = new UpdateCustomerCategoryCommand()
                    {
                        Id = Args.Data.Id,
                        Name = Args.Data.Name,
                    };
                    await GCApiProxy!.Proxy.CustomerCategory_UpdateCustomerCategoryAsync(Args.Data.Id, Command);

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
                await GCApiProxy!.Proxy.CustomerCategory_DeleteCustomerCategoryAsync(toDelete);

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
