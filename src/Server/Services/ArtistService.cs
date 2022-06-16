using BlazorMusic.Server.Models;
using BlazorMusic.Server.Providers;
using BlazorMusic.Shared;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Server.Services;

public class ArtistService : IArtistService
{
    private readonly IMusicDataProvider musicDataProvider;
    private readonly MusicDataOptions musicDataOptions;
    private readonly ILogger<ArtistService> logger;

    public ArtistService(IMusicDataProvider musicDataProvider, IOptions<MusicDataOptions> musicDataOptions, ILogger<ArtistService> logger)
    {
        this.musicDataProvider = musicDataProvider;

        this.musicDataOptions = musicDataOptions.Value ??
            throw new ArgumentNullException(nameof(musicDataOptions), "Missing music data options. Check configuration");

        this.logger = logger;
    }

    public Task<ArtistCollection> SearchArtists(string keyword, int page)
    {
        logger.LogDebug("Search artist {Keyword} on page {Page}", keyword, page);

        throw new NotImplementedException();
    }
}
