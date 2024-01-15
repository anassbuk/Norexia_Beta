using MediatR;

namespace Norexia.Core.Application.Currencies.Commands.UpdateCurrency;

public class UpdateCurrencyCommand : IRequest<Guid>
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}
