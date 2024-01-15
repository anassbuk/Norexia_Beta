using System.ComponentModel.DataAnnotations;

namespace Norexia.Core.Domain.Common.Enums;
public enum PaymentOption
{
    [Display(Name = "Paiement à la livraison")]
    CashOnDelivery = 0,
    [Display(Name = "Paiement à la commande (anticipé)")]
    PaymentOnOrder = 1,
    [Display(Name = "Paiement sur facture")]
    PaymentOnInvoice = 2
}
