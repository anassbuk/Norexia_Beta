using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Payments.Commands.CreatePayment;
using Norexia.Core.Application.Payments.Commands.DeletePayment;
using Norexia.Core.Application.Payments.Commands.UpdatePayment;
using Norexia.Core.Application.Payments.Queries.GetInvoicePayments;
using Norexia.Core.Application.Payments.Queries.GetPayments;
using Norexia.Core.Application.Payments.Queries.GetSaleOrderAsPayment;
using Norexia.Core.Application.Payments.Queries.GetSaleOrderPayments;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
public class PaymentController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreatePayment([FromBody] CreatePaymentCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<PaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPaymentsQuery(), cancellationToken);

        return Ok(result);
    }


    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdatePayment([FromRoute] Guid id, [FromBody] UpdatePaymentCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeletePayment([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePaymentCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{id:Guid}/invoice-payments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<PaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>?>> GetInvoicePayments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetInvoicePaymentsQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/sale-payments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<PaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>?>> GetSaleOrderPayments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSaleOrderPaymentsQuery(id), cancellationToken);

        return Ok(result);
    }


    [HttpGet("{term}/sale-as-payment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SaleOrderAsPaymentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SaleOrderAsPaymentDto?>> GetSaleOrderAsPayment(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetSaleOrderAsPaymentQuery(term), cancellationToken);

        return Ok(result);
    }
}
