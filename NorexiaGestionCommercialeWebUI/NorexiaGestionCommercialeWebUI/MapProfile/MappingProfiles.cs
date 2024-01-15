using AutoMapper;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Client;
using NorexiaGestionCommercialeWebUI.Models.CreditNote;
using NorexiaGestionCommercialeWebUI.Models.Delivery;
using NorexiaGestionCommercialeWebUI.Models.Invoice;
using NorexiaGestionCommercialeWebUI.Models.Payment;
using NorexiaGestionCommercialeWebUI.Models.Product;
using NorexiaGestionCommercialeWebUI.Models.Provider;
using NorexiaGestionCommercialeWebUI.Models.Purchase;
using NorexiaGestionCommercialeWebUI.Models.Quotation;
using NorexiaGestionCommercialeWebUI.Models.Sale;
using NorexiaGestionCommercialeWebUI.Models.StockEntry;

namespace NorexiaGestionCommercialeWebUI.MapProfile;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateProductCommand, ProductCommand>().ReverseMap();
        CreateMap<UpdateProductCommand, ProductCommand>().ReverseMap();
        CreateMap<ProductDetailsDto, ProductCommand>().ReverseMap();

        CreateMap<CreateCustomer, ClientCommand>().ReverseMap();
        CreateMap<UpdateCustomer, ClientCommand>().ReverseMap();
        CreateMap<CustomerAddressDto, CreateCustomerAddress>().ReverseMap();
        CreateMap<CustomerAddressDto, UpdateCustomerAddress>().ReverseMap();
        CreateMap<ClientCommand, CustomerDetailsDto>().ReverseMap();

        CreateMap<CreateProvider, ProviderCommand>().ReverseMap();
        CreateMap<UpdateProvider, ProviderCommand>().ReverseMap();
        CreateMap<ProviderAddressDto, CreateProviderAddress>().ReverseMap();
        CreateMap<ProviderAddressDto, UpdateProviderAddress>().ReverseMap();
        CreateMap<ProviderCommand, ProviderDetailsDto>().ReverseMap();

        CreateMap<CreateSaleOrderCommand, SaleCommand>().ReverseMap();
        CreateMap<UpdateSaleOrderCommand, SaleCommand>().ReverseMap();
        CreateMap<SaleOrderLineCommand, SaleOrderLineDto>().ReverseMap();
        CreateMap<SaleOrderPaymentCommand, PaymentDto>().ReverseMap();
        CreateMap<SaleOrderDto, SaleCommand>().ReverseMap();
        

        CreateMap<CreateQuotationCommand, QuotationCommand>().ReverseMap();
        CreateMap<UpdateQuotationCommand, QuotationCommand>().ReverseMap();
        CreateMap<QuotationLineCommand, QuotationLineDto>().ReverseMap();
        CreateMap<QuotationDto, QuotationCommand>().ReverseMap();

        
        CreateMap<CreateCreditNoteCommand, CreditNoteCommand>().ReverseMap();
        CreateMap<UpdateCreditNoteCommand, CreditNoteCommand>().ReverseMap();
        CreateMap<CreditNoteLineCommand, CreditNoteLineDto>().ReverseMap();
        CreateMap<CreditNoteDto, CreditNoteCommand>().ReverseMap();

      

        CreateMap<CreatePurchaseOrderCommand, PurchaseCommand>().ReverseMap();
        CreateMap<UpdatePurchaseOrderCommand, PurchaseCommand>().ReverseMap();
        CreateMap<PurchaseOrderLineCommand, PurchaseOrderLineDto>().ReverseMap();
        CreateMap<PurchaseOrderDto, PurchaseCommand>().ReverseMap();

        CreateMap<CreateStockEntryCommand, StockEntryCommand>().ReverseMap();
        CreateMap<UpdateStockEntryCommand, StockEntryCommand>().ReverseMap();
        CreateMap<StockEntryLineCommand, StockEntryLineDto>().ReverseMap();
        CreateMap<StockEntryDto, StockEntryCommand>().ReverseMap();

        CreateMap<CreateDelivererCommand, DelivererDto>().ReverseMap();
        CreateMap<UpdateDelivererCommand, DelivererDto>().ReverseMap();

        CreateMap<CreateDeliveryCommand, DeliveryCommand>().ReverseMap();
        CreateMap<UpdateDeliveryCommand, DeliveryCommand>().ReverseMap();
        CreateMap<DeliveryLineCommand, DeliveryLineDto>().ReverseMap();
        CreateMap<DeliveryDto, DeliveryCommand>().ReverseMap();

        CreateMap<PaymentTermsDto, UpdatePaymentTermsCommand>().ReverseMap();

        CreateMap<OwnedPaymentTerms, PaymentTermsDto>().ReverseMap();

        CreateMap<CreateInvoiceCommand, InvoiceCommand>().ReverseMap();
        CreateMap<UpdateInvoiceCommand, InvoiceCommand>().ReverseMap();
        CreateMap<InvoiceLineCommand, InvoiceLineDto>().ReverseMap();
        CreateMap<InvoiceDto, InvoiceCommand>().ReverseMap();
        CreateMap<InvoicePaymentCommand, PaymentDto>().ReverseMap();

        CreateMap<CreatePaymentCommand, PaymentCommand>().ReverseMap();
        CreateMap<UpdatePaymentCommand, PaymentCommand>().ReverseMap();
        CreateMap<PaymentDto, PaymentCommand>().ReverseMap();

        CreateMap<PaymentDto, PaymentDto>()
            .ForMember(dest => dest.EntryDate,
                opt => opt.MapFrom(src => src.EntryDate!.Value.ToLocalTime()));

        CreateMap<CreditNoteDto, CreditNoteDto>()
            .ForMember(dest => dest.CreditNoteDate,
                           opt => opt.MapFrom(src => src.CreditNoteDate!.Value.ToLocalTime())).ForMember(dest =>dest.DueDate, opt => opt.MapFrom(src => src.CreditNoteDate!.Value.ToLocalTime()));
    }
}
