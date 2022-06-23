using BlazorMusic.Server.Extensions;
using BlazorMusic.Server.Models;
using BlazorMusic.Server.Providers;
using BlazorMusic.Shared;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Server.Services;

/// <inheritdoc cref="IReleaseService"/>
public class ReleaseService : IReleaseService
{
    private readonly IMusicDataProvider musicDataProvider;
    private readonly MusicDataOptions musicDataOptions;
    private readonly ILogger<ReleaseService> logger;

    public ReleaseService(
        IMusicDataProvider musicDataProvider,
        IOptions<MusicDataOptions> musicDataOptions,
        ILogger<ReleaseService> logger)
    {
        this.musicDataProvider = musicDataProvider;

        this.musicDataOptions = musicDataOptions.Value ??
            throw new ArgumentNullException(nameof(musicDataOptions), "Missing music data options. Check configuration");

        this.logger = logger;
    }

    public async Task<IEnumerable<ReleaseTrack>> RetrieveReleaseTracksByIdAsync(string id)
    {
        logger.LogDebug("Retrieving releases for artist {Id}", id);

        var releaseResponse = await musicDataProvider.GetReleaseByIdAsync(id);

        return releaseResponse.Media.FirstOrDefault()?.Tracks.ToTracks();
    }
}
