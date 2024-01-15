using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.Common.Enums;
using Norexia.Core.Domain.ProviderEntities;

namespace Norexia.Core.Application.Providers.Queries.GetProviders;

public class ProviderAddressDto : IMapFrom<ProviderAddress>
{
    public Guid? Id { get; set; }
    public AddressType? AddressType { get; set; }
    public string? StreetAdress { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Region { get; set; }
    public string? Localisation { get; set; }
    public string? Complement { get; set; }
    public bool? Active { get; set; }
}
