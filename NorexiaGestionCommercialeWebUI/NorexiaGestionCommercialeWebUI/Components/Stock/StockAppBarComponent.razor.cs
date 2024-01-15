using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.SplitButtons;

namespace NorexiaGestionCommercialeWebUI.Components.Stock;
public partial class StockAppBarComponent
{
    [Inject]
    private NavigationManager? Navigation { get; set; }
    public void OnSelect(string link)
    {
        Navigation!.NavigateTo(link);
    }
    private void ItemSelected(MenuEventArgs args)
    {
        if (args.Item.Id == "Entries")
            OnSelect("/Stock/Entries");
        else if (args.Item.Id == "Exits")
            OnSelect("/Stock/Exits");
    }
}
