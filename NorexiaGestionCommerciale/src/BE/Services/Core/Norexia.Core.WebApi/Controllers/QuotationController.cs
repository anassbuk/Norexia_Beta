using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Products.Queries.GetProductAsQuotationLine;
using Norexia.Core.Application.Quotations.Commands.CreateQuotation;
using Norexia.Core.Application.Quotations.Commands.DeletQuotation;
using Norexia.Core.Application.Quotations.Commands.UpdateQuotation;
using Norexia.Core.Application.Quotations.Queries.GetQuotations;
using Norexia.Core.Application.Quotations.Queries.GetQuotationsLines;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuotationController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateQuotation([FromBody] CreateQuotationCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<QuotationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<QuotationDto>>> GetQuotation(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetQuotationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:Guid}/Lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<QuotationLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<QuotationLineDto>>> GetQutationLines(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetQuotationsLinesQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateQoutation([FromRoute] Guid id, [FromBody] UpdateQuotationCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteQuotation([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeletQuotationommand(ids), cancellationToken);
        return Ok();
    }
    [HttpGet("{term}/as-quotation-line")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(QuotationLineDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<QuotationLineDto?>> GetProductAsQuotationLine(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAsQuotationLineQuery(term), cancellationToken);

        return Ok(result);
    }

}
