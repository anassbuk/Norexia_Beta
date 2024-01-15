using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.PaymentTerms.Commands;
using Norexia.Core.Application.PaymentTerms.Queries;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentTermsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PaymentTermsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaymentTermsDto?>> GetPaymentTerms(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPaymentTermsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdatePaymentTerms([FromBody] UpdatePaymentTermsCommand command, CancellationToken cancellationToken)
    {
        await Mediator.Send(command, cancellationToken);
        return Ok();
    }
}
