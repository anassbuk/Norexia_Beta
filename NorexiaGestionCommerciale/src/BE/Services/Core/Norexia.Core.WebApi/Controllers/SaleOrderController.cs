using Microsoft.AspNetCore.Mvc;
using Norexia.Core.Application.SaleOrders.Commands.CreateSaleOrder;
using Norexia.Core.Application.SaleOrders.Commands.DeleteSaleOrder;
using Norexia.Core.Application.SaleOrders.Commands.UpdateSaleOrder;
using Norexia.Core.Application.SaleOrders.Queries.GetQuotationAsSaleOrder;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderAsDelivery;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderAsInvoice;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrderLines;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SaleOrderController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateSaleOrder([FromBody] CreateSaleOrderCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<SaleOrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaleOrderDto>>> GetSaleOrders(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSaleOrdersQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<SaleOrderLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SaleOrderLineDto>>> GetSaleOrderLines([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSaleOrderLinesQuery(id), cancellationToken);

        return Ok(result);
    }


    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateSaleOrder([FromRoute] Guid id, [FromBody] UpdateSaleOrderCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteSaleOrder([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteSaleOrderCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{term}/as-delivery")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SaleOrderAsDeliveryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SaleOrderAsDeliveryDto?>> GetSaleOrderAsDelivery(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSaleOrderAsDeliveryQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/as-invoice")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SaleOrderAsInvoiceDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SaleOrderAsInvoiceDto?>> GetSaleOrderAsInvoice(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSaleOrderAsInvoiceQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/quotation-as-sale")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(QuotationAsSaleOrderDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<QuotationAsSaleOrderDto?>> GetQuotationAsSaleOrder(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetQuotationAsSaleOrderQuery(term), cancellationToken);

        return Ok(result);
    }
}
