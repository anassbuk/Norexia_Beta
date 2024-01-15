using Norexia.Core.Application.Common.Mappings;
using Norexia.Core.Domain.ProductEntities;

namespace Norexia.Core.Application.Products.Queries.GetProduct;
public class NoteDto : IMapFrom<ProductNote>
{
    public string? Note { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
}
