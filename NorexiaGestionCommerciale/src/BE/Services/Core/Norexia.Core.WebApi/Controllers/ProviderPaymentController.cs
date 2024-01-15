using Microsoft.AspNetCore.Mvc;
using Norexia.Core.Application.Payments.Queries.GetPayments;
using Norexia.Core.Application.ProviderPayments.Commands.CreateProviderPayment;
using Norexia.Core.Application.ProviderPayments.Commands.DeleteProviderPayment;
using Norexia.Core.Application.ProviderPayments.Commands.UpdateProviderPayment;
using Norexia.Core.Application.ProviderPayments.Queries.GetProviderInvoiceAsPayment;
using Norexia.Core.Application.ProviderPayments.Queries.GetProviderInvoicePayments;
using Norexia.Core.Application.ProviderPayments.Queries.GetProviderPayments;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]

public class ProviderPaymentController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateProviderPayment([FromBody] CreateProviderPaymentCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProviderInvoicePaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetProviderPayments(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderPaymentsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateProviderPayment([FromRoute] Guid id, [FromBody] UpdateProviderPaymentCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProviderPayment([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProviderPaymentCommand(ids), cancellationToken);

        return Ok();
    }


    [HttpGet("{id:Guid}/provider-invoice-payments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProviderInvoicePaymentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProviderInvoicePaymentDto>?>> GetProviderInvoicePayments([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderInvoicePaymentsQuery(id), cancellationToken);

        return Ok(result);
    }


    [HttpGet("{term}/provider-invoice-as-payment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProviderInvoiceAsPaymentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProviderInvoiceAsPaymentDto?>> GetProviderInvoiceAsPayment(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderInvoiceAsPaymentQuery(term), cancellationToken);

        return Ok(result);
    }
}
