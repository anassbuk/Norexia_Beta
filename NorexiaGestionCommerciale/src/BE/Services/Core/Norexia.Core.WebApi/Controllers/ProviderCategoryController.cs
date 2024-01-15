using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.ProviderCategories.Commands.CreateProviderCategory;
using Norexia.Core.Application.ProviderCategories.Commands.DeleteProviderCategory;
using Norexia.Core.Application.ProviderCategories.Commands.UpdateProviderCategory;
using Norexia.Core.Application.ProviderCategories.Queries.GetProviderCategories;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProviderCategoryController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProviderCategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProviderCategoryDto>>> GetProviderCategories(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderCategoriesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateProviderCategory([FromBody] CreateProviderCategoryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateProviderCategory([FromRoute] Guid id, [FromBody] UpdateProviderCategoryCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProviderCategory([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProviderCategoryCommand(ids), cancellationToken);

        return Ok();
    }
}
