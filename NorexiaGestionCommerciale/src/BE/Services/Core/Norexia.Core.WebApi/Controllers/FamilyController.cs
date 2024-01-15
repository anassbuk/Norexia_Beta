using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Families.Commands.CreateFamily;
using Norexia.Core.Application.Families.Commands.DeleteFamily;
using Norexia.Core.Application.Families.Commands.UpdateFamily;
using Norexia.Core.Application.Families.Queries.GetFamilies;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FamilyController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<FamilyDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FamilyDto>>> GetFamilies(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetFamiliesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateFamily([FromBody] CreateFamilyCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }


    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateFamily([FromRoute] Guid id, [FromBody] UpdateFamilyCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteFamily([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteFamilyCommand(ids), cancellationToken);

        return Ok();
    }
}
