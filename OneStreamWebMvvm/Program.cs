using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OneStreamWebUI.Mvvm.Toolkit;
using Syncfusion.Blazor;
using OneStreamWebMvvm;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDk1NDk5QDMxMzkyZTMyMmUzMEQrR1NTN05xSDNTWU5MajF3RFZ3T2Q3enJiN3NjTEdHMitMaEEwRHdqdGM9");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.OneStreamMvvm();
builder.Services.AddViewModels();
builder.Services.AddComponentServices();
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();
