using Microsoft.AspNetCore.Components;

namespace NorexiaGestionCommercialeWebUI.Shared;
public partial class MainLayout
{
    [Inject]
    NavigationManager? Navigation { get; set; }
    public EventCallback ToggleSidebarEvent => EventCallback.Factory.Create(this, ToggleSidebar);
    public bool SidebarToggle = false;

    public void ToggleSidebar()
    {
        SidebarToggle = !SidebarToggle;
        StateHasChanged();
    }
    public void OnSelect(Syncfusion.Blazor.Lists.ClickEventArgs<ListData> args)
    {
        Navigation!.NavigateTo(args.ItemData.PageLink);
    }

    public List<ListData> MainList = new ()
    {
        new ListData {Id="1", Text = "Accueil", IconCss = "sb-icons icon-home e-sb-icon control-icon sidebar-icon", PageLink = "/"},
        new ListData {Id="2",Text = "Produit", IconCss = "sb-icons icon-product e-sb-icon control-icon sidebar-icon", PageLink = "/Products"},
        new ListData {Id="3",Text = "Clients", IconCss = "sb-icons icon-people e-sb-icon control-icon sidebar-icon", PageLink = "/Clients"},
        new ListData {Id="4",Text = "fournisseurs", IconCss = "sb-icons icon-people e-sb-icon control-icon sidebar-icon", PageLink = "/Providers"},
        new ListData {Id="5",Text = "Ventes", IconCss = "sb-icons icon-sales e-sb-icon control-icon sidebar-icon", PageLink = "/Sales"},
        new ListData {Id="6",Text = "Achats", IconCss = "sb-icons icon-sales e-sb-icon control-icon sidebar-icon", PageLink = "/Purchases"},
        new ListData {Id="7",Text = "Stock", IconCss = "sb-icons icon-product e-sb-icon control-icon sidebar-icon", PageLink = "/Stock/Status"},
        new ListData {Id="8",Text = "Livraison", IconCss = "sb-icons icon-delivery e-sb-icon control-icon sidebar-icon", PageLink = "/Deliveries"},
        new ListData {Id="9",Text = "Factures", IconCss = "sb-icons icon-invoice e-sb-icon control-icon sidebar-icon", PageLink = "/Invoices"},
        new ListData {Id="10",Text = "Règlements", IconCss = "sb-icons icon-payment e-sb-icon control-icon sidebar-icon", PageLink = "/Payments"},
        new ListData {Id="11",Text = "Devis", IconCss = "sb-icons icon-invoice e-sb-icon control-icon sidebar-icon", PageLink = "/Quotations"},
    };

    public List<ListData> FooterList = new ()
    {
        new ListData {Id="1",Text = "Paramétrage", IconCss = "sb-icons icon-setting e-sb-icon control-icon sidebar-icon", PageLink = "/GeneralSettings"},
    };

    public class ListData
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string IconCss { get; set; } = string.Empty;
        public string PageLink { get; set; } = string.Empty;
    }
}
