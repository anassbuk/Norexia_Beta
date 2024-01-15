using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using System.Collections.ObjectModel;
using Syncfusion.Blazor.Grids;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Popups;

namespace NorexiaGestionCommercialeWebUI.Components.Settings;
public partial class FamilySettings
{
    private bool IsCreateDialogVisible { get; set; }
    private bool IsAddDialog { get; set; }
    [Inject]
    public GestionCommercialApiProxy? GCApiProxy { get; set; }

    public SelfReferentialFamily? DialogFamily;
    public ObservableCollection<SelfReferentialFamily>? Families;
    public List<SelfReferentialFamily>? ParentFamilies;
    public bool IsSubfamily { get; set; }

    private string DialogValidationMessage = string.Empty;

    private EditContext? EC { get; set; }

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
        DialogFamily = new();
        EC = new EditContext(DialogFamily!);
        Families = await InitFamilyTree();
        ParentFamilies = Families.Where(f => f.ParentFamilyId is null).ToList();
    }
    private async Task CreateFamilyClick()
    {
        if (EC!.Validate())
        {
            if (IsSubfamily)
            {
                if (DialogFamily!.ParentFamilyId is null)
                {
                    DialogValidationMessage = "Veuillez sélectionner une famille";
                    return;
                }
                else if (Families!.Any(f => f.ParentFamilyId == DialogFamily.ParentFamilyId && f.Designation == DialogFamily.Designation))
                {
                    DialogValidationMessage = $"La famille sélectionnée contient déjà une sous-famille avec le nom {DialogFamily.Designation}";
                    return;
                }
            }
            else if (Families!.Any(f => f.ParentFamilyId == null && f.Designation == DialogFamily!.Designation))
            {
                DialogValidationMessage = $"La famille avec la désignation {DialogFamily!.Designation} existe déjà";
                return;
            }

            try
            {
                var command = new CreateFamilyCommand()
                {
                    Designation = DialogFamily!.Designation,
                    ParentFamilyId = DialogFamily!.ParentFamilyId
                };

                var familyId = await GCApiProxy!.Proxy.Family_CreateFamilyAsync(command);
                DialogFamily!.FamilyId = familyId;
                Families!.Add(new SelfReferentialFamily()
                {
                    FamilyId = DialogFamily!.FamilyId,
                    Designation = DialogFamily!.Designation,
                    ParentFamilyId = DialogFamily!.ParentFamilyId,
                });
                if (DialogFamily!.ParentFamilyId is null)
                    ParentFamilies!.Add(new SelfReferentialFamily()
                    {
                        FamilyId = DialogFamily!.FamilyId,
                        Designation = DialogFamily!.Designation,
                        ParentFamilyId = DialogFamily!.ParentFamilyId,
                    });

                await ShowSuccessToast("Family added Successfully");
            }
            catch (Exception ex)
            {
                await ShowErrorToast(ex.Message);
            }

            this.IsSubfamily = false;
            this.IsCreateDialogVisible = false;
            DialogValidationMessage = string.Empty;
        }
    }

    private async Task EditFamilyClick()
    {
        if (IsSubfamily)
        {
            if (DialogFamily!.ParentFamilyId is null)
            {
                DialogValidationMessage = "Veuillez sélectionner une famille";
                return;
            }
            else if (Families!.Any(f => f.ParentFamilyId == DialogFamily.ParentFamilyId && f.Designation == DialogFamily.Designation))
            {
                DialogValidationMessage = $"La famille sélectionnée contient déjà une sous-famille avec le nom {DialogFamily.Designation}";
                return;
            }
        }
        else if (Families!.Any(f => f.ParentFamilyId == null && f.Designation == DialogFamily!.Designation))
        {
            DialogValidationMessage = $"La famille avec la désignation {DialogFamily!.Designation} existe déjà";
            return;
        }

        if (EC!.Validate())
        {
            try
            {
                var command = new UpdateFamilyCommand()
                {
                    Id = DialogFamily!.FamilyId,
                    Designation = DialogFamily!.Designation,
                    ParentFamilyId = DialogFamily!.ParentFamilyId
                };
                var familyId = await GCApiProxy!.Proxy.Family_UpdateFamilyAsync(DialogFamily!.FamilyId, command);

                var toUpdate = Families!.First(f => f.FamilyId == DialogFamily!.FamilyId);
                var index = Families!.IndexOf(toUpdate);
                if (index != -1)
                {

                    Families![index].Designation = DialogFamily!.Designation;
                    Families![index].ParentFamilyId = DialogFamily!.ParentFamilyId;
                }


                ParentFamilies = Families.Where(f => f.ParentFamilyId is null).ToList();

                await ShowSuccessToast("Family edited Successfully");
            }
            catch (Exception ex)
            {
                await ShowErrorToast(ex.Message);
            }

            this.IsSubfamily = false;
            this.IsCreateDialogVisible = false;
            DialogValidationMessage = string.Empty;
        }
    }

    public async Task<ObservableCollection<SelfReferentialFamily>> InitFamilyTree()
    {
        var familyDtos = await GCApiProxy!.Proxy.Family_GetFamiliesAsync();
        var flatFamilies = new ObservableCollection<SelfReferentialFamily>();
        foreach (var family in familyDtos)
        {
            flatFamilies.Add(new SelfReferentialFamily()
            {
                FamilyId = family.FamilyId,
                Designation = family.Designation,
                ParentFamilyId = null
            });
            foreach (var subFamily in family.SubFamilies!)
            {
                flatFamilies.Add(new SelfReferentialFamily()
                {
                    FamilyId = subFamily.FamilyId,
                    Designation = subFamily.Designation,
                    ParentFamilyId = family.FamilyId
                });
            }
        }
        return flatFamilies;
    }

    public async Task OnActionBegin(ActionEventArgs<SelfReferentialFamily> args)
    {
        if (args.RequestType == Syncfusion.Blazor.Grids.Action.Add)
        {
            IsAddDialog = true;
            args.Cancel = true;
            DialogFamily!.Designation = null;
            DialogFamily.ParentFamilyId = null;
            IsCreateDialogVisible = true;
            EC = new(DialogFamily);
        }
        else if (args.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
        {
            args.Cancel = true;
            IsAddDialog = false;
            DialogFamily!.FamilyId = args.Data.FamilyId;
            DialogFamily.Designation = args.Data.Designation;
            DialogFamily.ParentFamilyId = args.Data.ParentFamilyId;
            IsSubfamily = DialogFamily.ParentFamilyId != null;
            IsCreateDialogVisible = true;
            EC = new(DialogFamily);
        }
        else if (args.RequestType == Syncfusion.Blazor.Grids.Action.BeforeBeginEdit)
        {
            args.Cancel = true;
        }
        else if (args.RequestType == Syncfusion.Blazor.Grids.Action.Delete)
        {
            try
            {
                var family = args.Data;
                var familiesToDelete = new List<Guid>() { family.FamilyId };
                await GCApiProxy!.Proxy.Family_DeleteFamilyAsync(familiesToDelete);

                Families!.Remove(Families!.First(f => f.FamilyId == family.FamilyId));

                ParentFamilies = Families.Where(f => f.ParentFamilyId is null).ToList();

                await ShowSuccessToast("Family deleted Successfully");
            }
            catch (Exception ex)
            {
                await ShowErrorToast(ex.Message);
                args.Cancel = true;
            }
        }
    }

    public class SelfReferentialFamily : INotifyPropertyChanged
    {
        private Guid familyId;
        public Guid FamilyId
        {
            get { return familyId; }
            set
            {
                this.familyId = value;
                NotifyPropertyChanged(nameof(SelfReferentialFamily.FamilyId));
            }
        }
        public SelfReferentialFamily() { }

        private Guid? parentFamilyId;
        public Guid? ParentFamilyId
        {
            get { return parentFamilyId; }
            set
            {
                this.parentFamilyId = value;
                NotifyPropertyChanged(nameof(SelfReferentialFamily.ParentFamilyId));
            }
        }

        private string? designation;
        public string? Designation
        {
            get { return designation; }
            set
            {
                this.designation = value;
                NotifyPropertyChanged(nameof(SelfReferentialFamily.Designation));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public void DialogClosedHandler(CloseEventArgs args)
    {
        DialogValidationMessage = string.Empty;
    }
}
