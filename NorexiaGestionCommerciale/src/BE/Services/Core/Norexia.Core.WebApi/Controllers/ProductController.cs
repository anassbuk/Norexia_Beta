using Microsoft.AspNetCore.Mvc;

using Norexia.Core.Application.Deliveries.Queries.GetDeliveryLines;
using Norexia.Core.Application.Invoices.Queries.GetInvoiceLines;
using Norexia.Core.Application.Products.Commands.ActivateProduct;
using Norexia.Core.Application.Products.Commands.CreateProduct;
using Norexia.Core.Application.Products.Commands.DeleteProduct;
using Norexia.Core.Application.Products.Commands.UpdateProduct;
using Norexia.Core.Application.Products.Queries.GetProduct;
using Norexia.Core.Application.Products.Queries.GetProductAsDeliveryLine;
using Norexia.Core.Application.Products.Queries.GetProductAsInvoiceLine;
using Norexia.Core.Application.Products.Queries.GetProductAsPurchaseOrderLine;
using Norexia.Core.Application.Products.Queries.GetProductAsSellOrderLine;
using Norexia.Core.Application.Products.Queries.GetProductAsStockEntryLine;
using Norexia.Core.Application.Products.Queries.GetProductImages;
using Norexia.Core.Application.Products.Queries.GetProducts;
using Norexia.Core.Application.Products.Queries.SearchProducts;
using Norexia.Core.Application.PurchaseOrders.Queries.GetPurchaseOrderLines;
using Norexia.Core.Application.SaleOrders.Queries.GetSaleOrders;
using Norexia.Core.Application.StockEntries.Queries.GetStockEntryLines;
using Norexia.Core.Domain.Common.Models;
using Norexia.Core.WebApi.Controllers.common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Norexia.Core.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductController : ApiControllerBase
{
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProductDetailsDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductDetailsDto>> GetProduct(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductQuery(id), cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductsQuery(), cancellationToken);

        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> CreateProduct([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        return Ok(await Mediator.Send(command, cancellationToken));
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteProduct([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new DeleteProductCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpPut("UpdateProductStatus")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ActivateProduct([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await Mediator.Send(new ActivateProductCommand(ids), cancellationToken);

        return Ok();
    }

    [HttpGet("{term}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<ProductDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductDto>>> SearchProducts(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchProductsQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/as-sell-order-line")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SaleOrderLineDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SaleOrderLineDto?>> GetProductAsSellOrderLine(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAsSellOrderLineQuery(term), cancellationToken);

        return Ok(result);
    }



    [HttpGet("{term}/as-purchase-order-line")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(PurchaseOrderLineDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PurchaseOrderLineDto?>> GetProductAsPurchaseOrderLine(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAsPurchaseOrderLineQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/as-stock-entry-line")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(StockEntryLineDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<StockEntryLineDto?>> GetProductAsStockEntryLine(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAsStockEntryLineQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/as-delivery-line")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DeliveryLineDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<DeliveryLineDto?>> GetProductAsDeliveryLine(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAsDeliveryLineQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{term}/as-invoice-line")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InvoiceLineDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<InvoiceLineDto?>> GetProductAsInvoiceLine(string term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductAsInvoiceLineQuery(term), cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/images")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<FileBase64>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FileBase64>>> GetProductImages(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductImagesQuery(id), cancellationToken);

        return Ok(result);
    }
}
