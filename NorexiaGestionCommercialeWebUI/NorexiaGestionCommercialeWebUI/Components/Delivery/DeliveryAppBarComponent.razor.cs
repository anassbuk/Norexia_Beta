using Microsoft.AspNetCore.Components;

namespace NorexiaGestionCommercialeWebUI.Components.Delivery;
public partial class DeliveryAppBarComponent
{
    [Inject]
    private NavigationManager? Navigation { get; set; }
    public void OnSelect(string link)
    {
        Navigation!.NavigateTo(link);
    }
}
