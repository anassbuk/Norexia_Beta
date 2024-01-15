using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Charts;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Lists.Internal;
using System.Net;
using System.Text;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class UnitSettings
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<UnitDto>? UnitList { get; set; }
    public List<UnitMeasurementDto>? UnitMeasurementList { get; set; }

    private SfGrid<UnitDto>? UnitListGrid;
    private bool IsDialogVisible { get; set; }

    private string DialogValidationMessage = string.Empty;

    [Parameter]
    public EventCallback<string> OnShowSuccessToast { get; set; }

    protected async Task ShowSuccessToast(string content)
    {
        await OnShowSuccessToast.InvokeAsync(content);
    }

    [Parameter]
    public EventCallback<string> OnShowErrorToast { get; set; }

    protected async Task ShowErrorToast(string content)
    {
        await OnShowErrorToast.InvokeAsync(content);
    }

    protected override async Task OnInitializedAsync()
    {
        UnitList = (List<UnitDto>)await GCApiProxy!.Proxy.Unit_GetUnitsAsync();
    }

    public async Task OnUnitActionBegin(ActionEventArgs<UnitDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (UnitList!.Any(c => c.Name == Args.Data.Name))
            {
                Args.Cancel = true;
                DialogValidationMessage = $"L'unité avec le nom {Args.Data.Name} existe déjà";
                this.IsDialogVisible = true;
            }
            else
            {
                if (Args.Action == "Add")
                {
                    try
                    {
                        var selectedClass = await UnitListGrid!.GetSelectedRecordsAsync();
                        var createUnitCommand = new CreateUnitTypeCommand()
                        {
                            Name = Args.Data.Name,
                        };
                        Guid addedUnitId = await GCApiProxy!.Proxy.Unit_CreateUnitAsync(createUnitCommand);
                        Args.Data.Id = addedUnitId;
                        await ShowSuccessToast("Unit Added Successfully");
                    }
                    catch (Exception ex)
                    {
                        await ShowErrorToast(ex.Message);
                        Args.Cancel = true;
                    }
                }
                else
                {
                    try
                    {
                        var updateUnitCommand = new UpdateUnitTypeCommand()
                        {
                            Id = Args.Data.Id,
                            Name = Args.Data.Name,
                        };
                        Guid updatedClassId = await GCApiProxy!.Proxy.Unit_UpdateUnitAsync(Args.Data.Id, updateUnitCommand);

                        await ShowSuccessToast("Unit Edited Successfully");
                    }
                    catch (Exception ex)
                    {
                        await ShowErrorToast(ex.Message);
                        Args.Cancel = true;
                    }
                }
            }
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                var unitsToDelete = new List<Guid>() { Args.Data.Id };
                await GCApiProxy!.Proxy.Unit_DeleteUnitAsync(unitsToDelete);

                await ShowSuccessToast("Unit deleted Successfully");
            }
            catch (Exception ex)
            {
                await ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }

    public async Task RowSelectHandler(RowSelectEventArgs<UnitDto> args)
    {
        UnitMeasurementList = (List<UnitMeasurementDto>)await GCApiProxy!.Proxy.Unit_GetUnitMeasurementsAsync(args.Data.Id);
    }

    public async Task OnUnitMeasurementActionBegin(ActionEventArgs<UnitMeasurementDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add ||
            Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete ||
            Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            var selectedRecords = await UnitListGrid!.GetSelectedRecordsAsync();

            if (selectedRecords.Count == 0)
            {
                Args.Cancel = true;
                DialogValidationMessage = "Veuillez sélectionner une unité dans la grille des unités";
                this.IsDialogVisible = true;
            }
        }
        else
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {

                if (UnitMeasurementList!.Any(c => c.Name == Args.Data.Name))
                {
                    Args.Cancel = true;
                    DialogValidationMessage = $"L'unité sélectionnée contient déjà la valeur {Args.Data.Name}";
                    this.IsDialogVisible = true;
                }
                else
                {
                    if (Args.Action == "Add")
                    {
                        try
                        {
                            Guid unitId;
                            var selectedRecords = await UnitListGrid!.GetSelectedRecordsAsync();

                            if (selectedRecords.Count == 0)
                            {
                                this.IsDialogVisible = true;
                                DialogValidationMessage = "Veuillez sélectionner une unité dans la grille des unités";
                                Args.Cancel = true;
                            }
                            unitId = selectedRecords.First().Id;
                            var createUnitMeasurementCommand = new CreateUnitMeasurementCommand()
                            {
                                UnitId = unitId,
                                Name = Args.Data.Name,
                            };
                            Guid addedUnitMeasurementId = await GCApiProxy!.Proxy.Unit_CreateUnitMeasurementAsync(createUnitMeasurementCommand, $"{unitId}");

                            await ShowSuccessToast("Unit measurement Added Successfully");
                        }
                        catch (Exception ex)
                        {
                            await ShowErrorToast(ex.Message);
                            Args.Cancel = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            Guid unitId;
                            var selectedRecords = await UnitListGrid!.GetSelectedRecordsAsync();

                            if (selectedRecords.Count == 0)
                            {
                                this.IsDialogVisible = true;
                                DialogValidationMessage = "Veuillez sélectionner une unité dans la grille des unités";
                                Args.Cancel = true;
                            }
                            unitId = selectedRecords.First().Id;
                            var updateUnitMeasurementCommand = new UpdateUnitMeasurementCommand()
                            {
                                Id = Args.Data.Id,
                                UnitId = unitId,
                                Name = Args.Data.Name,
                            };
                            Guid updatedUnitId = await GCApiProxy!.Proxy.Unit_UpdateUnitMeasurementAsync(Args.Data.Id, updateUnitMeasurementCommand, $"{unitId}");

                            await ShowSuccessToast("Unit measurement edited Successfully");
                        }
                        catch (Exception ex)
                        {
                            await ShowErrorToast(ex.Message);
                            Args.Cancel = true;
                        }
                    }
                }

            }
            else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
            {
                try
                {
                    Guid unitId;
                    var selectedRecords = await UnitListGrid!.GetSelectedRecordsAsync();

                    if (selectedRecords.Count == 0)
                    {
                        this.IsDialogVisible = true;
                        DialogValidationMessage = "Veuillez sélectionner une unité dans la grille des unités";
                        Args.Cancel = true;
                    }
                    unitId = selectedRecords.First().Id;

                    var unitMeasurementToDelete = new List<Guid>() { Args.Data.Id };
                    await GCApiProxy!.Proxy.Unit_DeleteUnitMeasurementAsync(unitMeasurementToDelete, $"{unitId}");

                    await ShowSuccessToast("Unit measurement deleted Successfully");
                }
                catch (Exception ex)
                {
                    await ShowErrorToast(ex.Message);
                    Args.Cancel = true;
                }
            }
        }

    }

    private void DialogOkClick()
    {
        this.IsDialogVisible = false;
    }
}
