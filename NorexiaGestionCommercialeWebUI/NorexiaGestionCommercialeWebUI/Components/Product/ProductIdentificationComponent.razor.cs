using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Product;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Product;
public partial class ProductIdentificationComponent
{
    readonly List<DropDownProductType> ddProductTypes = new()
    {
        new DropDownProductType() { DisplayName = "Produit", ProductType = ProductType.Product},
        new DropDownProductType() { DisplayName = "Service", ProductType = ProductType.Service},
        new DropDownProductType() { DisplayName = "Consommable", ProductType = ProductType.Consumable},
    };

    List<ProductNote> ProductNotes = new();
    string[]? SelectedAvailabilities;
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public SfMultiSelect<string[], ProductAvailabilityDto>? SfMultiSelect;

    [Parameter]
    public List<ProductAvailabilityDto> ProductAvailablities { get; set; } = new();

    [Parameter]
    public ProductCommand? ProductCommand { get; set; }

    [Parameter]
    public EventCallback<ProductCommand> ProductCommandChanged { get; set; }

    public async Task HandleProductIdentificationInfo()
    {
        ProductCommand!.ProductNotes = ProductNotes.Where(n => n.IsNew == true).Select(p => new NoteDto()
        {
            Note = p.Note,
            Created = p.Created,
            CreatedBy = p.CreatedBy,
        }).ToList();

        await ProductCommandChanged.InvokeAsync(ProductCommand);
    }

    protected override void OnInitialized()
    {
        if (ProductCommand!.ProductAvailabilities is null)
            ProductCommand!.ProductAvailabilities = new List<ProductAssignedAvailabilityDto>();
        InitiateComponentData();
    }

    private void AvailabilityValueRemovehandler(RemoveEventArgs<ProductAvailabilityDto> args)
    {
        var availability = ProductCommand!.ProductAvailabilities!.First(a => a.ProductAvailabilityId == args.ItemData.Id);
        ProductCommand!.ProductAvailabilities!.Remove(availability);
    }

    private void AvailabilityChipSelectedhandler(SelectEventArgs<ProductAvailabilityDto> args)
    {
        ProductCommand!.ProductAvailabilities!.Add(new ProductAssignedAvailabilityDto() { ProductAvailabilityId = args.ItemData.Id });
    }

    public void InitiateComponentData()
    {
        SelectedAvailabilities = ProductAvailablities.Where(al => ProductCommand!.ProductAvailabilities!.Any(ar => ar.ProductAvailabilityId == al.Id)).Select(al => al.Name).ToArray();

        if (ProductCommand!.ProductNotes != null)
            ProductNotes = ProductCommand!.ProductNotes.Select(n => new ProductNote
            {
                Note = n.Note,
                Created = n.Created.DateTime,
                CreatedBy = n.CreatedBy,
                IsNew = false
            }).ToList();
    }
}

public class ProductNote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool IsNew { get; set; } = true;
    public string Note { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; } = "Admin";
}

public class DropDownProductType
{
    public string? DisplayName { get; set; }
    public ProductType ProductType { get; set; }
}
