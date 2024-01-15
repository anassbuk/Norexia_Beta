using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using Syncfusion.Blazor.Grids;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class ClassSettings
{
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }
    public List<ClassDto>? ClassList { get; set; }
    public List<ClassValueDto>? ClassValueList { get; set; }

    private SfGrid<ClassDto>? ClassListGrid;
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
        ClassList = (List<ClassDto>)await GCApiProxy!.Proxy.Class_GetClassesAsync();
    }

    public async Task OnClassActionBegin(ActionEventArgs<ClassDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
        {
            if (ClassList!.Any(c => c.Key == Args.Data.Key))
            {
                Args.Cancel = true;
                DialogValidationMessage = $"La classe avec le nom {Args.Data.Key} existe déjà";
                this.IsDialogVisible = true;
            }
            else
            {
                if (Args.Action == "Add")
                {
                    try
                    {
                        var createClassKeyCommand = new CreateClassKeyCommand()
                        {
                            Key = Args.Data.Key,
                        };
                        Guid addedClassId = await GCApiProxy!.Proxy.Class_CreateClassAsync(createClassKeyCommand);

                        Args.Data.Id = addedClassId;
                        await ShowSuccessToast("Class Added Successfully");
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
                        var updateClassKeyCommand = new UpdateClassKeyCommand()
                        {
                            Id = Args.Data.Id,
                            Key = Args.Data.Key,
                        };
                        Guid updatedClassId = await GCApiProxy!.Proxy.Class_UpdateClassAsync(Args.Data.Id, updateClassKeyCommand);

                        await ShowSuccessToast("Class Edited Successfully");
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
                var classesToDelete = new List<Guid>() { Args.Data.Id };
                await GCApiProxy!.Proxy.Class_DeleteClassAsync(classesToDelete);

                await ShowSuccessToast("Class deleted Successfully");
            }
            catch (Exception ex)
            {
                await ShowErrorToast(ex.Message);
                Args.Cancel = true;
            }
        }
    }

    public async Task RowSelectHandler(RowSelectEventArgs<ClassDto> args)
    {
        ClassValueList = (List<ClassValueDto>)await GCApiProxy!.Proxy.Class_GetClassValuesAsync(args.Data.Id);
    }

    public async Task OnClassValueActionBegin(ActionEventArgs<ClassValueDto> Args)
    {
        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Add ||
            Args.RequestType == Syncfusion.Blazor.Grids.Action.Delete ||
            Args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            var selectedRecords = await ClassListGrid!.GetSelectedRecordsAsync();

            if (selectedRecords.Count == 0)
            {
                Args.Cancel = true;
                DialogValidationMessage = "Veuillez sélectionner une classe dans la grille des classes.";
                this.IsDialogVisible = true;
            }
        }
        else
        {
            if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save)
            {
                Guid classId;
                var selectedRecords = await ClassListGrid!.GetSelectedRecordsAsync();

                if (selectedRecords.Count == 0)
                {
                    Args.Cancel = true;
                    DialogValidationMessage = "Veuillez sélectionner une classe dans la grille des classes.";
                    this.IsDialogVisible = true;
                }

                classId = selectedRecords.First().Id;

                if (ClassValueList!.Any(c => c.Value == Args.Data.Value))
                {
                    Args.Cancel = true;
                    DialogValidationMessage = $"la classe sélectionnée contient déjà la valeur {Args.Data.Value}";
                    this.IsDialogVisible = true;
                }
                else
                {
                    if (Args.Action == "Add")
                    {
                        try
                        {
                            var createClassValueCommand = new CreateClassValueCommand()
                            {
                                ClassId = classId,
                                Value = Args.Data.Value,
                            };
                            Guid addedClassValueId = await GCApiProxy!.Proxy.Class_CreateClassValueAsync(createClassValueCommand, $"{classId}");

                            await ShowSuccessToast("Class value Added Successfully");
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
                            var updateClassValueCommand = new UpdateClassValueCommand()
                            {
                                Id = Args.Data.Id,
                                ClassId = classId,
                                Value = Args.Data.Value,
                            };
                            Guid updatedClassId = await GCApiProxy!.Proxy.Class_UpdateClassValueAsync(Args.Data.Id, updateClassValueCommand, $"{classId}");

                            await ShowSuccessToast("Class value edited Successfully");
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
                    Guid classId;
                    var selectedRecords = await ClassListGrid!.GetSelectedRecordsAsync();

                    if (selectedRecords.Count == 0)
                    {
                        DialogValidationMessage = "Veuillez sélectionner une classe dans la grille des classes.";
                        this.IsDialogVisible = true;
                        Args.Cancel = true;
                    }
                    classId = selectedRecords.First().Id;
                    var classValuesToDelete = new List<Guid>() { Args.Data.Id };
                    await GCApiProxy!.Proxy.Class_DeleteClassValueAsync(classValuesToDelete, $"{classId}");

                    await ShowSuccessToast("Class value deleted Successfully");
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
