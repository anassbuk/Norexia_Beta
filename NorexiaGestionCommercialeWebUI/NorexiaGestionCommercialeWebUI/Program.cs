using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NorexiaGestionCommercialeWebUI;
using Syncfusion.Blazor;
using Norexia.Core.Facade.Client.Sdk;
using NorexiaGestionCommercialeWebUI.Proxies;
using NorexiaGestionCommercialeWebUI.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NorexiaGestionCommercialeWebUI.Shared;
using System.Globalization;
using System.Net.NetworkInformation;
using NorexiaGestionCommercialeWebUI.AppState;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSyncfusionBlazor();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration.GetValue<string>("SyncfusionLicenseKey"));

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddHttpClient<GestionCommercialApiProxy>(); 
builder.Services
    .Configure<GestionCommercialApiSettings>(builder.Configuration.GetSection(GestionCommercialApiSettings.SettingName));
builder.Services.AddSingleton(typeof(ISyncfusionStringLocalizer), typeof(SyncfusionLocalizer));
builder.Services.AddSingleton<States>();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("fr");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("fr");

await builder.Build().RunAsync();
