using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.CreditNotes.Queries.GetInvoiceAsCreditNote;
using Norexia.Core.Application.Invoices.Commands.CreateInvoice;
using Norexia.Core.Application.Invoices.Commands.DeleteInvoice;
using Norexia.Core.Application.Invoices.Commands.UpdateInvoice;
using Norexia.Core.Application.Invoices.Queries.GetInvoiceAsDelivery;
using Norexia.Core.Application.Invoices.Queries.GetInvoiceAsPayment;
using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Application.Invoices.Queries.GetInvoices;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateInvoice([FromBody] CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetInvoicesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<InvoiceLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<InvoiceLineDto>>> GetInvoiceLines([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetInvoiceLinesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateInvoice([FromRoute] Guid id, [FromBody] UpdateInvoiceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteInvoice([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteInvoiceCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{term}/as-delivery")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InvoiceAsDeliveryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InvoiceAsDeliveryDto?>> GetInvoiceAsDelivery(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetInvoiceAsDeliveryQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/as-payment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InvoiceAsPaymentDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InvoiceAsPaymentDto?>> GetInvoiceAsPayment(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetInvoiceAsPaymentQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/as-credit-note")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InvoiceAsCreditNoteDto), StatusCodes.Status200OK)]
public async Task<ActionResult<InvoiceAsCreditNoteDto?>> GetInvoiceAsCreditNote(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetInvoiceAsCreditNoteQuery(term), cancellationToken);

        return Ok(result);
    }
}
