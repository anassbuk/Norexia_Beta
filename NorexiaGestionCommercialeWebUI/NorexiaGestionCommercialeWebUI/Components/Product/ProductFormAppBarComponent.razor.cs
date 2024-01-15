using AutoMapper;
using Microsoft.AspNetCore.Components;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Models;
using NorexiaGestionCommercialeWebUI.Proxies;

namespace NorexiaGestionCommercialeWebUI.Components.Product;
public partial class ProductFormAppBarComponent
{
    [Parameter]
    public EventCallback OnSaveProductClicked { get; set; }

    protected async Task SaveProductClicked()
    {
        await OnSaveProductClicked.InvokeAsync();
    }
}
