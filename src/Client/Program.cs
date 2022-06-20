using BlazorFocused;
using BlazorMusic.Client;
using BlazorMusic.Client.Actions;
using BlazorMusic.Client.Models;
using BlazorMusic.Client.Reducers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var hostEnvironmentBaseAddress = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = hostEnvironmentBaseAddress });

builder.Services
    .AddOptions<ApiOptions>()
    .BindConfiguration(nameof(ApiOptions));

builder.Services.AddRestClient(client =>
{
    client.BaseAddress = hostEnvironmentBaseAddress;
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddStore<ArtistStore>(new())
    .AddTransient<SearchArtistAction>()
    .AddTransient<SelectArtistAction>()
    .AddTransient<HistogramDataReducer>();

await builder.Build().RunAsync();
