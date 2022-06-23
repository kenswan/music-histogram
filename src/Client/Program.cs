using BlazorFocused;
using BlazorMusic.Client;
using BlazorMusic.Client.Actions;
using BlazorMusic.Client.Models;
using BlazorMusic.Client.Provider;
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
    client.Timeout = TimeSpan.FromSeconds(20);
});

builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();

builder.Services.AddStore<ArtistStore>(new())
    // Actions
    .AddTransient<SearchArtistAction>()
    .AddTransient<SelectArtistAction>()
    .AddTransient<AttachTracksAction>()
    .AddTransient<RetrieveReleasesAction>()
    .AddTransient<TogglePreviewAction>()
    // Reducers
    .AddTransient<ArtistReleasesReducer>()
    .AddTransient<ArtistHistogramReducer>();

await builder.Build().RunAsync();
