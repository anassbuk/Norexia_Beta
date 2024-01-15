using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.UnitMeasurements.Commands.CreateUnitMeasurement;
using Norexia.Core.Application.UnitMeasurements.Commands.DeleteUnitMeasurement;
using Norexia.Core.Application.UnitMeasurements.Commands.UpdateUnitMeasurement;
using Norexia.Core.Application.UnitMeasurements.Queries;
using Norexia.Core.Application.UnitTypes.Commands.CreateUnitType;
using Norexia.Core.Application.UnitTypes.Commands.DeleteUnitType;
using Norexia.Core.Application.UnitTypes.Commands.UpdateUnitType;
using Norexia.Core.Application.UnitTypes.Queries.GetUnits;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnitController : ApiControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<UnitDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UnitDto>>> GetUnits(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUnitsQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateUnit([FromBody] CreateUnitTypeCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateUnit([FromRoute] Guid id, [FromBody] UpdateUnitTypeCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteUnit([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteUnitTypeCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{id:Guid}/measurements")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<UnitMeasurementDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UnitMeasurementDto>>> GetUnitMeasurements([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetUnitMeasurementsQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:Guid}/measurement")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateUnitMeasurement([FromBody] CreateUnitMeasurementCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{unitid:Guid}/measurement/{measurementid:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateUnitMeasurement([FromRoute] Guid measurementid, [FromBody] UpdateUnitMeasurementCommand command, CancellationToken cancellationToken)
    {
        if (measurementid != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id:Guid}/measurements")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteUnitMeasurement([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteUnitMeasurementCommand(ids), cancellationToken);

        return Ok();
    }
}
