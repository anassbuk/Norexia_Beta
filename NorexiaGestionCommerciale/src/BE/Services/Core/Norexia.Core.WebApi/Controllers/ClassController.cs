using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.ClassKeys.Commands.CreateClassKey;
using Norexia.Core.Application.ClassKeys.Commands.DeleteClassKey;
using Norexia.Core.Application.ClassKeys.Commands.UpdateClassKey;
using Norexia.Core.Application.ClassKeys.Queries.GetClasses;
using Norexia.Core.Application.ClassValues.Commands.CreateClassValue;
using Norexia.Core.Application.ClassValues.Commands.DeleteClassValue;
using Norexia.Core.Application.ClassValues.Commands.UpdateClassValue;
using Norexia.Core.Application.ClassValues.Queries;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ClassDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetClassesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateClass([FromBody] CreateClassKeyCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }


    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateClass([FromRoute] Guid id, [FromBody] UpdateClassKeyCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteClass([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteClassKeyCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{id:Guid}/values")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ClassValueDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClassValueDto>>> GetClassValues([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetClassValuesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:Guid}/value")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateClassValue([FromBody] CreateClassValueCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{classid:Guid}/value/{valueid:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateClassValue([FromRoute] Guid valueid, [FromBody] UpdateClassValueCommand command, CancellationToken cancellationToken)
    {
        if (valueid != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:Guid}/values")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteClassValue([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteClassValueCommand(ids), cancellationToken);

        return Ok();
    }
}
