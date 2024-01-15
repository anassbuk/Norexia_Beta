using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.PriceGroups.Commands.CreatePriceGroup;
using Norexia.Core.Application.PriceGroups.Commands.DeletePriceGroup;
using Norexia.Core.Application.PriceGroups.Commands.UpdatePriceGroup;
using Norexia.Core.Application.PriceGroups.Queries.GetDefaultPriceGroup;
using Norexia.Core.Application.PriceGroups.Queries.GetPriceGroups;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PriceGroupController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<PriceGroupDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PriceGroupDto>>> GetPriceGroups(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPriceGroupsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreatePriceGroup([FromBody] CreatePriceGroupCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdatePriceGroup([FromRoute] Guid id, [FromBody] UpdatePriceGroupCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeletePriceGroup([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletePriceGroupCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("/default")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> GetDefaultPriceGroup(CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(new GetDefaultPriceGroupQuery(), cancellationToken));
    }
}
