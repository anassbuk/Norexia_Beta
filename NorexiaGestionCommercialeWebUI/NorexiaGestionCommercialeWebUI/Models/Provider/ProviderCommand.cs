using Norexia.Core.Facade.Client.Sdk;

namespace NorexiaGestionCommercialeWebUI.Models.Provider;
public class ProviderCommand
{
    public Guid Id { get; set; }
    public string? Reference { get; set; }
    public ProviderType? ProviderType { get; set; }

    //Contact
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Tel { get; set; }
    public string? Fax { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public string? Function { get; set; }

    //Company Contact & details
    public string? SocialReason { get; set; }
    public string? ICE { get; set; }
    public string? CompanyTel { get; set; }
    public string? CompanyFax { get; set; }
    public string? CompanyEmail { get; set; }
    public string? WebSite { get; set; }
    public string? Location { get; set; }

    public byte? Active { get; set; }
    public Guid? ProviderCategoryId { get; set; }
    public List<ProviderAddressDto> ProviderAddresses { get; set; } = new List<ProviderAddressDto>();
}
