using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.CreditNotes.Commands.CreateCreditNote;
using Norexia.Core.Application.CreditNotes.Commands.DeleteCreditNote;
using Norexia.Core.Application.CreditNotes.Commands.UpdateCrediNote;
using Norexia.Core.Application.CreditNotes.Queries.GetCreditNoteLines;
using Norexia.Core.Application.CreditNotes.Queries.GetCreditNotes;
using Norexia.Core.Application.Invoices.Queries.CreditNoteLineDto;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CreditNoteController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateCreditNote([FromBody] CreateCreditNoteCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }


    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CreditNoteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CreditNoteDto>>> GetCreditNotes(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCreditNotesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<CreditNoteLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CreditNoteLineDto>>> GetInvoiceLines([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCreditNoteLinesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteCreditNotes([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteCreditNoteCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateCreditNotes([FromRoute] Guid id, [FromBody] UpdateCreditNoteCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }


}
