using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Deliveries.Commands.CreateDelivery;
using Norexia.Core.Application.Deliveries.Commands.DeleteDelivery;
using Norexia.Core.Application.Deliveries.Commands.UpdateDelivery;
using Norexia.Core.Application.Deliveries.Queries.GetDeliveries;
using Norexia.Core.Application.Deliveries.Queries.GetDeliveriesAsInvoice;
using Norexia.Core.Application.Deliveries.Queries.GetDeliveryAsInvoice;
using Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;
using Norexia.Core.Domain.Common.Models;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateDelivery([FromBody] CreateDeliveryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<DeliveryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DeliveryDto>>> GetDeliveries(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDeliveriesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<DeliveryLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DeliveryLineDto>>> GetDeliveryLines([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDeliveryLinesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateDelivery([FromRoute] Guid id, [FromBody] UpdateDeliveryCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteDelivery([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteDeliveryCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{term}/as-invoice")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DeliveryAsInvoiceDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DeliveryAsInvoiceDto?>> GetDeliveryAsInvoice(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDeliveryAsInvoiceQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{term}/deliveries-as-invoice")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DeliveriesAsInvoiceDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DeliveriesAsInvoiceDto?>> GetDeliveriesAsInvoice(string term, [FromBody] FromToDate fromToDate, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDeliveriesAsInvoiceQuery(term, fromToDate.StartDate, fromToDate.EndDate), cancellationToken);

        return Ok(result);
    }
}
