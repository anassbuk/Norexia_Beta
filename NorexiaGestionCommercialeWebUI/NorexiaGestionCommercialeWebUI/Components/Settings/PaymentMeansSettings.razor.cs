using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Settings
{
    public partial class PaymentMeansSettings
    {
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }
        public List<PaymentMeanDto>? PaymentMeans { get; set; }
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
            PaymentMeans = (List<PaymentMeanDto>)await GCApiProxy!.Proxy.PaymentMean_GetPaymentMeansAsync();
        }

        public async Task OnMeanActionBegin(ActionEventArgs<PaymentMeanDto> Args)
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                if (PaymentMeans!.Any(f => f.Name == Args.Data.Name))
                {
                    DialogValidationMessage = $"Moyen de paiement avec le nom {Args.Data.Name} existe déjà";
                    Args.Cancel = true;
                    return;
                }
                if (Args.Action == "Add")
                {
                    try
                    {
                        var createCommand = new CreatePaymentMeanCommand()
                        {
                            Name = Args.Data.Name,
                        };
                        Guid addedId = await GCApiProxy!.Proxy.PaymentMean_CreatePaymentMeanAsync(createCommand);
                        Args.Data.Id = addedId;
                        await ShowSuccessToast("Payment mean added Successfully");
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
                        var updateCommand = new UpdatePaymentMeanCommand()
                        {
                            Id = Args.Data.Id,
                            Name = Args.Data.Name,
                        };
                        await GCApiProxy!.Proxy.PaymentMean_UpdatePaymentMeanAsync(Args.Data.Id, updateCommand);

                        await ShowSuccessToast("Payment mean Edited Successfully");
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
                    await GCApiProxy!.Proxy.PaymentMean_DeletePaymentMeanAsync(toDelete);

                    await ShowSuccessToast("Payment mean deleted Successfully");
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
