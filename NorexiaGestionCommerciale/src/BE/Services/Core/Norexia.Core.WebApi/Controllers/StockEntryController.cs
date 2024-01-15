using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.StockEntries.Commands.CreateStockEntry;
using Norexia.Core.Application.StockEntries.Commands.DeleteStockEntry;
using Norexia.Core.Application.StockEntries.Commands.UpdateStockEntry;
using Norexia.Core.Application.StockEntries.Queries.GetStockEntries;
using Norexia.Core.Application.StockEntries.Queries.GetStockEntryLines;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StockEntryController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateStockEntry([FromBody] CreateStockEntryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<StockEntryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StockEntryDto>>> GetStockEntries(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetStockEntriesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateStockEntry([FromRoute] Guid id, [FromBody] UpdateStockEntryCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet("{id:Guid}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<StockEntryLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<StockEntryLineDto>>> GetStockEntryLines([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetStockEntryLinesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteStockEntry([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteStockEntryCommand(ids), cancellationToken);

        return Ok();
    }
}
