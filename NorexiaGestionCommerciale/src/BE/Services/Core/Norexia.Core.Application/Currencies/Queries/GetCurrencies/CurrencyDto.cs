using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.SettingEntities;

namespace Norexia.Core.Application.Currencies.Queries.GetCurrencies;

public class CurrencyDto : IMapFrom<Currency>
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public bool IsDefault { get; set; }
}
