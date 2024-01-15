using Microsoft.AspNetCore.Mvc;
using Norexia.Core.Application.ProviderInvoices.Commands.CreateProviderInvoice;
using Norexia.Core.Application.ProviderInvoices.Commands.DeleteProviderInvoice;
using Norexia.Core.Application.ProviderInvoices.Commands.UpdateProviderInvoice;
using Norexia.Core.Application.ProviderInvoices.Queries.GetAttachedDigitalInvoices;
using Norexia.Core.Application.ProviderInvoices.Queries.GetProductAsProviderInvoiceLine;
using Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoiceLines;
using Norexia.Core.Application.ProviderInvoices.Queries.GetProviderInvoices;
using Norexia.Core.Application.ProviderInvoices.Queries.GetPurchaseOrderAsProviderInvoice;
using Norexia.Core.WebApi.Controllers.common;

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]

public class ProviderInvoiceController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateProviderInvoice([FromBody] CreateProviderInvoiceCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProviderInvoiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProviderInvoiceDto>>> GetProviderInvoices(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderInvoicesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/lines")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProviderInvoiceLineDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProviderInvoiceLineDto>>> GetProviderInvoiceLines([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProviderInvoiceLinesQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/attached-digital-invoices")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<AttachedDigitalInvoiceDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AttachedDigitalInvoiceDto>>> GetAttachedDigitalInvoices([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAttachedDigitalInvoicesQuery(id), cancellationToken);

        return Ok(result);
    }


    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateProviderInvoice([FromRoute] Guid id, [FromBody] UpdateProviderInvoiceCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProviderInvoice([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProviderInvoiceCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{term}/product-as-invoice-line")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProviderInvoiceLineDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProviderInvoiceLineDto?>> GetProductAsProviderInvoiceLine(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAsProviderInvoiceLineQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/purchase-as-invoice")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PurchaseOrderAsProviderInvoiceDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PurchaseOrderAsProviderInvoiceDto?>> GetPurchaseOrderAsProviderInvoice(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPurchaseOrderAsProviderInvoiceQuery(term), cancellationToken);

        return Ok(result);
    }
}
