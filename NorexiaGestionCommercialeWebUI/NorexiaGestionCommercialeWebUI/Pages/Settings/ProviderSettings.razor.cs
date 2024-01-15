using NorexiaGestionCommercialeWebUI.Components;
using Syncfusion.Blazor.Notifications;

namespace NorexiaGestionCommercialeWebUI.Pages.Settings;
public partial class ProviderSettings
{
    ToastComponent? Toast;
    private async Task ShowSuccessToast(string content)
    {
        await Toast!.ShowSuccessToast(content);
    }
    private async Task ShowErrorToast(string content)
    {
        await Toast!.ShowErrorToast(content);
    }
}
