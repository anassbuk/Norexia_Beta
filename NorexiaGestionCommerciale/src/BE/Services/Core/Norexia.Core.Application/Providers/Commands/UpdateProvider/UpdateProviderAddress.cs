using Norexia.Core.Domain.Common.Enums;

namespace Norexia.Core.Application.Providers.Commands.UpdateProvider;
public class UpdateProviderAddress
{
    public Guid Id { get; set; }
    public AddressType? AddressType { get; set; }
    public string? StreetAdress { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? Localisation { get; set; }
    public string? Complement { get; set; }
    public bool? Active { get; set; }
}
