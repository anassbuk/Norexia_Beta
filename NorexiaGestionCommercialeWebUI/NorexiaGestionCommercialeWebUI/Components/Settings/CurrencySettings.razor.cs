using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor;

namespace NorexiaGestionCommercialeWebUI.Components.Settings
{
    public partial class CurrencySettings
    {
        [Parameter]
        public List<CurrencyDto>? Currencies { get; set; }
        public CurrencyDto? DefaultCurrency { get; set; } = new();
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        private string DialogValidationMessage = string.Empty;
        private EditContext? EC;

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
        protected override void OnInitialized()
        {
            EC = new EditContext(DefaultCurrency!);
        }

        protected override void OnParametersSet()
        {
            if (Currencies != null && Currencies!.Any(v => v.IsDefault))
            {
                DefaultCurrency = Currencies!.FirstOrDefault(v => v.IsDefault);
                Currencies!.Remove(DefaultCurrency!);
                EC = new EditContext(DefaultCurrency!);
            }
        }

        private async Task DefaultCurrencyValueChanged(ChangeEventArgs args)
        {
            if (EC!.Validate())
            {
                try
                {

                    var updateCommand = new UpdateCurrencyCommand()
                    {
                        Id = DefaultCurrency!.Id,
                        Name = (string?)args.Value,
                    };

                    Guid guid = (Guid)DefaultCurrency!.Id!;

                    await GCApiProxy!.Proxy.Currency_UpdateCurrencyAsync((Guid)DefaultCurrency!.Id!, updateCommand);

                    await ShowSuccessToast("Default Currency edited Successfully");

                }
                catch (Exception ex)
                {
                    await ShowErrorToast(ex.Message);
                }
            }
        }
        public async Task OnCurrencyActionBegin(ActionEventArgs<CurrencyDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                var currenciesWithDefaultVat = new List<CurrencyDto>(Currencies!);
                currenciesWithDefaultVat!.Add(DefaultCurrency!);
                if (currenciesWithDefaultVat!.Any(f => f.Name == Args.Data.Name))
                {
                    DialogValidationMessage = $"Devise avec le nom {Args.Data.Name!} existe déjà";
                    Args.Cancel = true;
                    return;
                }
                if (Args.Action == "Add")
                {
                    try
                    {
                        var createCommand = new CreateCurrencyCommand()
                        {
                            Name = Args.Data.Name,
                        };
                        Guid addedId = await GCApiProxy!.Proxy.Currency_CreateCurrencyAsync(createCommand);
                        Args.Data.Id = addedId;
                        await ShowSuccessToast("Currency added Successfully");
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
                        var updateCommand = new UpdateCurrencyCommand()
                        {
                            Id = Args.Data.Id,
                            Name = Args.Data.Name,
                        };
                        await GCApiProxy!.Proxy.Currency_UpdateCurrencyAsync((Guid)Args.Data.Id!, updateCommand);

                        await ShowSuccessToast("Currency Edited Successfully");
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
                    var toDelete = new List<Guid>() { (Guid)Args.Data.Id! };
                    await GCApiProxy!.Proxy.Currency_DeleteCurrencyAsync(toDelete);

                    await ShowSuccessToast("Currency deleted Successfully");
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
}
