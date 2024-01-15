using MediatR;

namespace Norexia.Core.Application.Currencies.Commands.CreateCurrency;

public class CreateCurrencyCommand : IRequest<Guid>
{
    public string? Name { get; set; }
}
