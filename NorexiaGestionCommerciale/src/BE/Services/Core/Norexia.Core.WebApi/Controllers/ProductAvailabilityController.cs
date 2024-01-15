using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.ProductAvailabilities.Commands.CreateProductAvailability;
using Norexia.Core.Application.ProductAvailabilities.Commands.DeleteProductAvailability;
using Norexia.Core.Application.ProductAvailabilities.Commands.UpdateProductAvailability;
using Norexia.Core.Application.ProductAvailabilities.Queries.GetProductAvailabilities;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductAvailabilityController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProductAvailabilityDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductAvailabilityDto>>> GetProductAvailabilities(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAvailabilitiesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateProductAvailability([FromBody] CreateProductAvailabilityCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateProductAvailability([FromRoute] Guid id, [FromBody] UpdateProductAvailabilityCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProductAvailability([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProductAvailabilityCommand(ids), cancellationToken);

        return Ok();
    }
}
