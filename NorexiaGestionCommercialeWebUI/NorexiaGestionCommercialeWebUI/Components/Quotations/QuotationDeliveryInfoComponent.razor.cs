using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Quotation;

namespace NorexiaGestionCommercialeWebUI.Components.Quotations
{
    public partial class QuotationDeliveryInfoComponent
    {
        [Parameter]
        public QuotationCommand? QuotationCommand { get; set; }
        [Parameter]
        public EventCallback<QuotationCommand> QuotationCommandChanged { get; set; }

        public List<DropDownDeliveryMode> ddDeliveryMode = new()
        {
            new DropDownDeliveryMode() { DisplayName = "Récupération au magasin", DeliveryMode = DeliveryMode.PickUpAtStore},
            new DropDownDeliveryMode() { DisplayName = "Livraison à domicile", DeliveryMode = DeliveryMode.HomeDelivery},
        };  
    }

    public class DropDownDeliveryMode
    {
        public string? DisplayName { get; set; }
        public DeliveryMode? DeliveryMode { get; set; }
    }
      
}

