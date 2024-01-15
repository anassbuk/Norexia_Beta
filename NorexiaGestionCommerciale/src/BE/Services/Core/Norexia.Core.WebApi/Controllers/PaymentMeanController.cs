using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.PaymentMeans.Commands.CreatePaymentMean;
using Norexia.Core.Application.PaymentMeans.Commands.DeletePaymentMean;
using Norexia.Core.Application.PaymentMeans.Commands.UpdatePaymentMean;
using Norexia.Core.Application.PaymentMeans.Queries.GetPaymentMeans;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentMeanController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<PaymentMeanDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentMeanDto>>> GetPaymentMeans(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPaymentMeansQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreatePaymentMean([FromBody] CreatePaymentMeanCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdatePaymentMean([FromRoute] Guid id, [FromBody] UpdatePaymentMeanCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeletePaymentMean([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePaymentMeanCommand(ids), cancellationToken);

        return Ok();
    }
}
