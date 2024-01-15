using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Norexia.Core.Facade.Client.Sdk;
using Syncfusion.Blazor.Inputs;
using static System.Net.Mime.MediaTypeNames;
using System;
using Syncfusion.Blazor.RichTextEditor.Internal;
using Syncfusion.Blazor.Navigations;
using Syncfusion.ExcelExport;
using Syncfusion.Blazor.Popups;
using NorexiaGestionCommercialeWebUI.Models.Product;

namespace NorexiaGestionCommercialeWebUI.Components.Product;
public partial class ProductGeneralInformationComponent
{
    [Parameter]
    public ProductCommand? ProductCommand { get; set; }

    [Parameter]
    public EventCallback<ProductCommand> ProductCommandChanged { get; set; }

    private bool? CanBeSold = false;
    private bool? CanBePurchased = false;
    private bool IsImageDialogVisible;
    const long maxFileSize = 5000000; // 5 MB
    private SfUploader? SfUploader;
    private SfDialog? ImagesDialog;

    protected async override Task OnInitializedAsync()
    {
        if (ProductCommand!.ProductImages is null)
            ProductCommand!.ProductImages = new List<FileBase64>();
        await InitiateComponentData();
        StateHasChanged();
    }

    public void OnProductActionChanged(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool?> args)
    {
        if (CanBeSold == true)
        {
            if (CanBePurchased == true)
                ProductCommand!.Action = ProductAction.All;
            else
                ProductCommand!.Action = ProductAction.Sale;
        }
        else if (CanBePurchased == true)
        {
            ProductCommand!.Action = ProductAction.Purchase;
        }
        else
        {
            ProductCommand!.Action = null;
        }
        //await ProductCommandChanged.InvokeAsync(ProductCommand);
    }


    public async Task InitiateComponentData()
    {
        if (ProductCommand!.Action is null)
        {
            CanBeSold = true; CanBePurchased = true;
            ProductCommand!.Action = ProductAction.All;
            await ProductCommandChanged.InvokeAsync(ProductCommand);
        }
        else if (ProductCommand!.Action == ProductAction.All)
        {
            CanBeSold = true; CanBePurchased = true;
        }
        else if (ProductCommand!.Action == ProductAction.Purchase)
        {
            CanBeSold = false; CanBePurchased = true;
        }
        else if (ProductCommand!.Action == ProductAction.Sale)
        {
            CanBeSold = true; CanBePurchased = false;
        }
    }

    public void ShowImagesEditDialog(MouseEventArgs args)
    {
        IsImageDialogVisible = true;
    }

    private async Task OnImageUploaderChangeHandler(UploadChangeEventArgs args)
    {
        foreach (var file in args.Files)
        {
            var buffer = new byte[file.File.Size];
            await file.File.OpenReadStream(maxFileSize).ReadAsync(buffer);

            var fileBase64 = new FileBase64()
            {
                Name = file.File.Name,
                ContentType = file.File.ContentType,
                Base64Data = Convert.ToBase64String(buffer)
            };

            ProductCommand!.ProductImages!.Add(fileBase64);
        }

        //await ProductCommandChanged.InvokeAsync(ProductCommand);
    }

    private void OnFileRemove(string name)
    {
        var image = ProductCommand!.ProductImages!.FirstOrDefault(i => i.Name == name);
        if (image != null)
        {
            ProductCommand!.ProductImages!.Remove(image);
            IsImageDialogVisible = false;
            //IsImageDialogVisible = true;

            //await ProductCommandChanged.InvokeAsync(ProductCommand);
        }
    }
}
