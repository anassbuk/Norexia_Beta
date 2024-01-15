using Microsoft.AspNetCore.Components;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class SettingAppBarComponent
{
    [Inject]
    private NavigationManager? Navigation { get; set; }
    public void OnSelect(string link)
    {
        Navigation!.NavigateTo(link);
    }
}