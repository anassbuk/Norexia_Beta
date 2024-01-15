using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.CustomerCategories.Commands.CreateCustomerCategory;
using Norexia.Core.Application.CustomerCategories.Commands.DeleteCustomerCategory;
using Norexia.Core.Application.CustomerCategories.Commands.UpdateCustomerCategory;
using Norexia.Core.Application.CustomerCategories.Queries.GetCustomerCategories;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomerCategoryController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CustomerCategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerCategoryDto>>> GetCustomerCategories(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCustomerCategoriesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateCustomerCategory([FromBody] CreateCustomerCategoryCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateCustomerCategory([FromRoute] Guid id, [FromBody] UpdateCustomerCategoryCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCustomerCategory([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCustomerCategoryCommand(ids), cancellationToken);

        return Ok();
    }
}
