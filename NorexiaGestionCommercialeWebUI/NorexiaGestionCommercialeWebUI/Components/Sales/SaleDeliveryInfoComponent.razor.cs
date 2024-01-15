using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Sale;

namespace NorexiaGestionCommercialeWebUI.Components.Sales
{
    public partial class SaleDeliveryInfoComponent
    {
        [Parameter]
        public SaleCommand? SaleCommand { get; set; }
        [Parameter]
        public EventCallback<SaleCommand> SaleCommandChanged { get; set; }

        readonly List<DropDownDeliveryMode> ddDeliveryMode = new()
        {
            new DropDownDeliveryMode() { DisplayName = "Récupération au magasin", DeliveryMode = DeliveryMode.PickUpAtStore},
            new DropDownDeliveryMode() { DisplayName = "Livraison à domicile", DeliveryMode = DeliveryMode.HomeDelivery},
        };

        public class DropDownDeliveryMode
        {
            public string? DisplayName { get; set; }
            public DeliveryMode DeliveryMode { get; set; }
        }
    }
}
