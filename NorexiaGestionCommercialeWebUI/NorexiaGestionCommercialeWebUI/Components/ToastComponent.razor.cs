using Syncfusion.Blazor.Notifications;

namespace NorexiaGestionCommercialeWebUI.Components;
public partial class ToastComponent
{
    SfToast? ToastObj;
    private readonly List<ToastModel> Toast = new()
    {
        new ToastModel{ Title = "Success!", CssClass="e-toast-success", Icon="e-success toast-icons" },
        new ToastModel{ Title = "Warning!", CssClass="e-toast-warning", Icon="e-warning toast-icons" },
        new ToastModel{ Title = "Error!", CssClass="e-toast-danger", Icon="e-error toast-icons" },
        new ToastModel{ Title = "Information!", CssClass="e-toast-info", Icon="e-info toast-icons" }
    };

    public async Task ShowSuccessToast(string content)
    {
        Toast[0].Content = content;
        await this.ToastObj!.ShowAsync(Toast[0]);
    }
    private async Task ShowWarnToast(string content)
    {
        Toast[1].Content = content;
        await this.ToastObj!.ShowAsync(Toast[0]);
    }
    public async Task ShowErrorToast(string content)
    {
        Toast[2].Content = content;
        await this.ToastObj!.ShowAsync(Toast[2]);
    }
    private async Task ShowInfoToast(string content)
    {
        Toast[3].Content = content;
        await this.ToastObj!.ShowAsync(Toast[3]);
    }
}
