using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Product;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using static NorexiaGestionCommercialeWebUI.Components.Product.ProductUnitComponent;

namespace NorexiaGestionCommercialeWebUI.Components.Product;
public partial class ProductClassificationComponent
{
    [Parameter]
    public ProductCommand? ProductCommand { get; set; }

    [Parameter]
    public EventCallback<ProductCommand> ProductCommandChanged { get; set; }

    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public List<FamilyDto>? Families { get; set; } = new();
    List<FamilyDto>? SubFamilies { get; set; } = new();
    [Parameter]
    public List<ClassDto>? Classes { get; set; } = new();
    Guid SelectedFamilyId { get; set; }
    ClassDto? SelectedClass { get; set; } = new();
    List<ClassValue>? ClassValues { get; set; } = new();
    private string DialogValidationMessage = string.Empty;

    protected override void OnInitialized()
    {
        if (ProductCommand!.ClassificationInfo is null)
            ProductCommand.ClassificationInfo = new();
    }
    protected override void OnParametersSet()
    {
        InitiateComponentData();
    }
    public async Task HandleProductClassificationInfo()
    {
        ProductCommand!.ProductClassifications = ClassValues!.Select(c => new ProductClassDto()
        {
            ProductClassValueId = c.ClassValueId
        }).ToList();

        await ProductCommandChanged.InvokeAsync(ProductCommand);
    }

    public void InitiateComponentData()
    {
        if (ProductCommand!.ClassificationInfo!.FamilyId != null)
        {
            var selectedFamily = Families!.FirstOrDefault(f=> f.SubFamilies.Any(s=>s.FamilyId == ProductCommand.ClassificationInfo.FamilyId));
            if (selectedFamily != null)
            {
                SubFamilies = selectedFamily.SubFamilies.ToList();
                SelectedFamilyId = selectedFamily.FamilyId;
            }
        }

        if (ProductCommand!.ProductClassifications != null)
        {
            List<ClassValue> classValues = new();
            foreach (var value in ProductCommand!.ProductClassifications)
            {
                ClassValue classValue = new();
                ClassDto classDto = Classes!.First(c => c.Values.Any(v => v.Id == value.ProductClassValueId));
                classValue.ClassValueId = value.ProductClassValueId;
                classValue.ClassId = classDto.Id;
                classValue.Value = classDto.Values.First(v => v.Id == value.ProductClassValueId).Value;
                classValue.ClassKey = classDto.Key;
                classValues!.Add(classValue);
            }
            ClassValues = classValues;
        }
    }

    public void OnFamilyChanged(SelectEventArgs<FamilyDto> args)
    {
        SubFamilies = args.ItemData.SubFamilies.ToList();
    }

    public void OnClassChanged(SelectEventArgs<ClassDto> args)
    {
        SelectedClass = args.ItemData;
    }

    public void OnClassActionBegin(ActionEventArgs<ClassValue> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (ClassValues!.Any(c => c.ClassId == Args.Data.ClassId && c.ClassValueId == Args.Data.ClassValueId))
            {
                Args.Cancel = true;
                DialogValidationMessage = "La classification sélectionnée déjà utilisée";
            }
            else
            {
                Args.Data.ClassKey = SelectedClass!.Key;
                Args.Data.Value = SelectedClass!.Values.First(v => v.Id == Args.Data.ClassValueId).Value;
            }
        }
    }

    public void OnClassActionComplete(ActionEventArgs<ClassValue> args)
    {
        if (args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add) || args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.BeginEdit))
            args.PreventRender = false;
        DialogValidationMessage = string.Empty;
    }

    public class ClassValue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ClassId { get; set; }
        public string? ClassKey { get; set; }
        public Guid ClassValueId { get; set; }
        public string? Value { get; set; }
    }
}
