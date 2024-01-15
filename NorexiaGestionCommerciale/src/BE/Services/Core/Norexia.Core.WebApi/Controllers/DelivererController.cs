using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Deliverers.Commands.ActivateDeliverer;
using Norexia.Core.Application.Deliverers.Commands.CreateDeliverer;
using Norexia.Core.Application.Deliverers.Commands.DeleteDeliverer;
using Norexia.Core.Application.Deliverers.Commands.UpdateDeliverer;
using Norexia.Core.Application.Deliverers.Queries.GetDeliverers;
using Norexia.Core.Application.Deliverers.Queries.GetProviderByReferenceOrName;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DelivererController : ApiControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateDeliverer([FromBody] CreateDelivererCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<DelivererDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DelivererDto>>> GetDeliverers(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDeliverersQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateDeliverer([FromRoute] Guid id, [FromBody] UpdateDelivererCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteDeliverer([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteDelivererCommand(ids), cancellationToken);
        return Ok();
    }

    [HttpPut("UpdateDelivererStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ActivateDeliverer([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ActivateDelivererCommand(ids), cancellationToken);

        return Ok();
    }



    [HttpGet("{term}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DelivererDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DelivererDto?>> GetDelivererByReferenceOrName(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDelivererByReferenceOrNameQuery(term), cancellationToken);

        return Ok(result);
    }

}
