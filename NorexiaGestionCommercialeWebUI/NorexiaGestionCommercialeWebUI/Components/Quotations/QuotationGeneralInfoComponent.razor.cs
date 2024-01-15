using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Quotation;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NorexiaGestionCommercialeWebUI.Components.Sales.SalesGeneralInfoComponent;

namespace NorexiaGestionCommercialeWebUI.Components.Quotations
{
    public partial class QuotationGeneralInfoComponent
    {
        [Inject]
        public GestionCommercialApiProxy? GCApiProxy { get; set; }

        [Parameter]
        public QuotationCommand? QuotationCommand { get; set; }
        [Parameter]
        public CustomerDetailsDto? Customer { get; set; }

        [Parameter]
        public EventCallback<QuotationCommand> QuotationCommandChanged { get; set; }

        private CustomerAddressDto? BillingCustomerAddress;
        private CustomerAddressDto? DeliveryCustomerAddress;
        private string? clientSearchTerm;
        private bool IsDialogVisible;
        private string DialogMessage = string.Empty;
        private bool isPassing = true;

        protected override void OnParametersSet()
        {
            DisplayClientAddress();
        }

        public async Task SearchClient(MouseEventArgs args)
        {
            await GetClient();
        }
        private async Task OnExecutionChange(ChangeEventArgs<SaleExecution?, DropDownSaleExecution> args)
        {
            await QuotationCommandChanged.InvokeAsync(QuotationCommand);
        }

        private async Task GetClient()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(clientSearchTerm) && GCApiProxy != null)
                {
                    Customer = await GCApiProxy.Proxy.Customer_GetCustomerByReferenceOrNameAsync(clientSearchTerm);
                    DisplayClientAddress();
                }
            }
            catch (Exception ex)
            {
                DialogMessage = $"Client avec le terme de recherche '{clientSearchTerm}' introuvable: {ex.Message}";
                IsDialogVisible = true;
            }
        }

        private void DisplayClientAddress()
        {
            if (Customer != null)
            {
                QuotationCommand!.CustomerId = Customer.Id;
                clientSearchTerm = $"{Customer.Reference} - {(Customer.ClientType == ClientType.Company ? Customer.SocialReason : $"{Customer.FirstName}, {Customer.LastName}")}";

                if (Customer.CustomerAddresses != null)
                {
                    BillingCustomerAddress = Customer.CustomerAddresses.FirstOrDefault(a => a.AddressType != AddressType.Delivery && a.Active == true);
                    DeliveryCustomerAddress = Customer.CustomerAddresses.FirstOrDefault(a => a.AddressType != AddressType.Billing && a.Active == true);
                }
            }
        }

        private void DialogOkClick()
        {
            this.IsDialogVisible = false;
        }
    }
}
