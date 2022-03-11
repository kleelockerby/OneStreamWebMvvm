using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OneStreamWebUI.Mvvm.Toolkit;
using OneStreamWebMvvm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.Services.OneStreamMvvm();
builder.Services.AddViewModels();
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IWeatherForecastService, WeatherForecastService>();

await builder.Build().RunAsync();
