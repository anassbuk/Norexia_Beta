using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace NorexiaGestionCommercialeWebUI.Components.Settings
{
    public partial class VATSettings
    {
        [Parameter]
        public List<VATDto>? VATs { get; set; }
        public VATDto? DefaultVAT { get; set; } = new();
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
            EC = new EditContext(DefaultVAT!);
        }

        protected override void OnParametersSet()
        {
            if (VATs != null && VATs!.Any(v => v.IsDefault))
            {
                DefaultVAT = VATs!.FirstOrDefault(v => v.IsDefault);
                VATs!.Remove(DefaultVAT!);
                EC = new EditContext(DefaultVAT!);
            }
        }

        private async Task DefaultVATValueChanged(ChangeEventArgs<decimal?> args)
        {
            if (EC!.Validate())
            {
                try
                {

                    var updateCommand = new UpdateVATCommand()
                    {
                        Id = DefaultVAT!.Id,
                        Value = args.Value,
                    };

                    Guid guid = (Guid)DefaultVAT!.Id!;

                    await GCApiProxy!.Proxy.VAT_UpdateVATAsync((Guid)DefaultVAT!.Id!, updateCommand);

                    await ShowSuccessToast("Default VAT edited Successfully");

                }
                catch (Exception ex)
                {
                    await ShowErrorToast(ex.Message);
                }
            }
        }
        public async Task OnVatActionBegin(ActionEventArgs<VATDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                var vatsWithDefaultVat = new List<VATDto>(VATs!);
                vatsWithDefaultVat!.Add(DefaultVAT!);
                if (vatsWithDefaultVat!.Any(f => f.Value == Args.Data.Value))
                {
                    DialogValidationMessage = $"TVA {Args.Data.Value!.Value:P2} existe déjà";
                    Args.Cancel = true;
                    return;
                }
                if (Args.Action == "Add")
                {
                    try
                    {
                        var createCommand = new CreateVATCommand()
                        {
                            Value = Args.Data.Value,
                        };
                        Guid addedId = await GCApiProxy!.Proxy.VAT_CreateVATAsync(createCommand);
                        Args.Data.Id = addedId;
                        await ShowSuccessToast("VAT added Successfully");
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
                        var updateCommand = new UpdateVATCommand()
                        {
                            Id = Args.Data.Id,
                            Value = Args.Data.Value,
                        };
                        await GCApiProxy!.Proxy.VAT_UpdateVATAsync((Guid)Args.Data.Id!, updateCommand);

                        await ShowSuccessToast("VAT Edited Successfully");
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
                    await GCApiProxy!.Proxy.VAT_DeleteVATAsync(toDelete);

                    await ShowSuccessToast("VAT deleted Successfully");
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
