using BlazorFocused;
using BlazorMusic.Server.Models;
using BlazorMusic.Server.Providers;
using BlazorMusic.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

// Add BlazorFocused REST client with appsettings.json configuration
builder.Services.AddRestClient();

builder.Services
    .AddOptions<MusicDataOptions>()
    .BindConfiguration(nameof(MusicDataOptions));

builder.Services.AddTransient<IArtistService, ArtistService>();
builder.Services.AddTransient<IMusicDataProvider, MusicDataProvider>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
