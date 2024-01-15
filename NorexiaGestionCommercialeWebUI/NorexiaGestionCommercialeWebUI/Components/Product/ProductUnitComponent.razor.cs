using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models.Product;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using System.Diagnostics.Metrics;

namespace NorexiaGestionCommercialeWebUI.Components.Product;
public partial class ProductUnitComponent
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    [Parameter]
    public ProductCommand? ProductCommand { get; set; }

    [Parameter]
    public EventCallback<ProductCommand> ProductCommandChanged { get; set; }
    [Parameter]
    public List<UnitDto>? Units { get; set; } = new();
    UnitDto? SelectedUnit { get; set; } = new();
    UnitDto? DialogSelectedUnit { get; set; } = new();
    UnitMeasurement? PrimaryUnitMeasurement { get; set; }
    List<UnitMeasurement>? UnitMeasurements { get; set; } = new();

    private EditContext? EC { get; set; }
    private string DialogValidationMessage = string.Empty;
    protected override void OnInitialized()
    {
        if (PrimaryUnitMeasurement is null)
            PrimaryUnitMeasurement = new();

        EC = new EditContext(PrimaryUnitMeasurement!);
        InitiateComponentData();

    }

    public void OnUnitChanged(SelectEventArgs<UnitDto> args)
    {
        SelectedUnit = args.ItemData;
    }

    public void OnDialogUnitChanged(SelectEventArgs<UnitDto> args)
    {
        DialogSelectedUnit = args.ItemData;
    }

    public void OnUnitActionBegin(ActionEventArgs<UnitMeasurement> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            var unitMeasurements = new List<UnitMeasurement>()
            {
                PrimaryUnitMeasurement!
            };
            unitMeasurements!.AddRange(UnitMeasurements!);
            if (unitMeasurements!.Any(m => m.UnitId == Args.Data.UnitId && m.MeasurementId == Args.Data.MeasurementId))
            {
                Args.Cancel = true;
                DialogValidationMessage = "L'unité de mesure sélectionnée déjà utilisée";
            }
            else
            {
                Args.Data.UnitName = DialogSelectedUnit!.Name;
                Args.Data.MeasurementName = DialogSelectedUnit!.Measurements.First(v => v.Id == Args.Data.MeasurementId).Name;
            }
        }
    }

    public void OnUnitActionComplete(ActionEventArgs<UnitMeasurement> args)
    {
        if (args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add) || args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.BeginEdit))
            args.PreventRender = false;
        DialogValidationMessage = string.Empty;
    }

    public async Task<bool> HandleProductUnitsInfo()
    {
        if (EC!.Validate())
        {
            List<ProductUnitDto> productUnits = new()
            {
                new ProductUnitDto() { ProductUnitMeasurementId = PrimaryUnitMeasurement!.MeasurementId }
            };

            productUnits.AddRange(UnitMeasurements!.Select(u => new ProductUnitDto()
            {
                ProductUnitMeasurementId = u.MeasurementId
            }).ToList());

            ProductCommand!.ProductUnits = productUnits;
            await ProductCommandChanged.InvokeAsync(ProductCommand);
            return true;
        }
        else
            return false;
    }

    public void InitiateComponentData()
    {
        if (ProductCommand!.ProductUnits != null)
        {
            List<UnitMeasurement> unitMeasurements = new();
            for (int i = 0; i < ProductCommand.ProductUnits.Count; i++)
            {
                var measurement = ProductCommand.ProductUnits.ToList()[i];
                UnitMeasurement unitMeasurement = new();
                UnitDto unitDto = Units!.First(u => u.Measurements.Any(m => m.Id == measurement.ProductUnitMeasurementId));
                unitMeasurement.MeasurementId = measurement.ProductUnitMeasurementId;
                unitMeasurement.UnitId = unitDto.Id;
                unitMeasurement.MeasurementName = unitDto.Measurements.First(m => m.Id == measurement.ProductUnitMeasurementId).Name;
                unitMeasurement.UnitName = unitDto.Name;

                if (i == 0)
                {
                    SelectedUnit = unitDto;
                    PrimaryUnitMeasurement!.UnitId = unitMeasurement.UnitId;
                    PrimaryUnitMeasurement!.MeasurementId = unitMeasurement.MeasurementId;
                }
                else
                    unitMeasurements!.Add(unitMeasurement);
            }
            UnitMeasurements = unitMeasurements;
        }
    }

    public class UnitMeasurement
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UnitId { get; set; }
        public string? UnitName { get; set; }
        public Guid MeasurementId { get; set; }
        public string? MeasurementName { get; set; }
    }
}
