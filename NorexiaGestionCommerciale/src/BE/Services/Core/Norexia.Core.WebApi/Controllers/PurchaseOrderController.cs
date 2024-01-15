using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.PurchaseOrders.Commands.CreatePurchaseOrder;
using Norexia.Core.Application.PurchaseOrders.Commands.DeletePurchaseOrder;
using Norexia.Core.Application.PurchaseOrders.Commands.UpdatePurchaseOrder;
using Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderAsStockEntry;
using Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderLines;
using Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrders;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PurchaseOrderController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreatePurchaseOrder([FromBody] CreatePurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<PurchaseOrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetPurchaseOrders(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPurchaseOrdersQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<PurchaseOrderLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PurchaseOrderLineDto>>> GetPurchaseOrderLines([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPurchaseOrderLinesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdatePurchaseOrder([FromRoute] Guid id, [FromBody] UpdatePurchaseOrderCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeletePurchaseOrder([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePurchaseOrderCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{term}/as-stock-entry")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PurchaseOrderAsStockEntryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PurchaseOrderAsStockEntryDto?>> GetPurchaseOrderAsStockEntry(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPurchaseOrderAsStockEntryQuery(term), cancellationToken);

        return Ok(result);
    }
}
