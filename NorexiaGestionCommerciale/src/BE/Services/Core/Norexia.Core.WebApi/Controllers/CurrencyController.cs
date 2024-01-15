using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Currencies.Commands.CreateCurrency;
using Norexia.Core.Application.Currencies.Commands.DeleteCurrency;
using Norexia.Core.Application.Currencies.Commands.UpdateCurrency;
using Norexia.Core.Application.Currencies.Queries.GetCurrencies;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CurrencyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetCurrencies(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCurrenciesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateCurrency([FromBody] CreateCurrencyCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateCurrency([FromRoute] Guid id, [FromBody] UpdateCurrencyCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCurrency([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCurrencyCommand(ids), cancellationToken);

        return Ok();
    }

}
