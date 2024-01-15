using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Customers.Commands.ActivateCustomer;
using Norexia.Core.Application.Customers.Commands.CreateCustomer;
using Norexia.Core.Application.Customers.Commands.DeleteCustomer;
using Norexia.Core.Application.Customers.Commands.UpdateCustomer;
using Norexia.Core.Application.Customers.Queries.GetCustomer;
using Norexia.Core.Application.Customers.Queries.GetCustomerByReferenceOrName;
using Norexia.Core.Application.Customers.Queries.GetCustomers;
using Norexia.Core.Application.Customers.Queries.SearchCustomers;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ApiControllerBase
{
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomerDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CustomerDetailsDto>> GetCustomer(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCustomerQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(CustomerDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<CustomerDetailsDto?>> GetCustomerByReferenceOrName(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCustomerByReferenceOrNameQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("SearchByName")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CustomerDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDetailsDto>>> SearchByName(string name, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchCustomerQueryByName(name), cancellationToken);

        return Ok(result);
    }

    [HttpGet("SearchByCategory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CustomerDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDetailsDto>>> SearchByCategory(Guid categorie, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchCustomerQueryByCategorie(categorie), cancellationToken);

        return Ok(result);
    }



    [HttpGet("SearchBySocialReason")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CustomerDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDetailsDto>>> SearchBySocialReason(string socialreason, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchCustomerQueryBySocialReason(socialreason), cancellationToken);

        return Ok(result);
    }



    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CustomerDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCustomersQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateCustomer([FromBody] CreateCustomer command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomer command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCustomer([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCustomer(ids), cancellationToken);
        return Ok();
    }

    [HttpPut("UpdateCustomerStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ActivateCustomer([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ActivateCustomerCommand(ids), cancellationToken);

        return Ok();
    }
}
