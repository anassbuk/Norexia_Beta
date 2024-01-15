using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Providers.Commands.ActivateProvider;
using Norexia.Core.Application.Providers.Commands.CreateProvider;
using Norexia.Core.Application.Providers.Commands.DeleteProvider;
using Norexia.Core.Application.Providers.Commands.UpdateProvider;
using Norexia.Core.Application.Providers.Queries.GetProvider;
using Norexia.Core.Application.Providers.Queries.GetProviderByReferenceOrName;
using Norexia.Core.Application.Providers.Queries.GetProviders;
using Norexia.Core.Application.Providers.Queries.SearchProviders;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProviderController : ApiControllerBase
{

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProviderDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProviderDetailsDto>> GetProvider(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProviderDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProviderDetailsDto?>> GetProviderByReferenceOrName(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderByReferenceOrNameQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProvidersDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProvidersDto>>> GetProviders(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProvidersQuery(), cancellationToken);

        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateProvider([FromBody] CreateProvider command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }


    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProvider([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProvider(ids), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateProvider([FromRoute] Guid id, [FromBody] UpdateProvider command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet("SearchBySocialReason")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProvidersDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProvidersDto>>> SearchBySocialReason(string socialreason, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchProviderQueryBySocialReason(socialreason), cancellationToken);

        return Ok(result);
    }

    [HttpPut("UpdateProviderStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ActivateProvider([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ActivateProviderCommand(ids), cancellationToken);

        return Ok();
    }
}
