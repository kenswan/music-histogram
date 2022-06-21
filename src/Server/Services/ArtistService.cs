﻿using BlazorMusic.Server.Extensions;
using BlazorMusic.Server.Models;
using BlazorMusic.Server.Providers;
using BlazorMusic.Shared;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Server.Services;

/// <inheritdoc cref="IArtistService"/>
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

    public async Task<IEnumerable<ArtistRelease>> RetrieveAllAristReleasesAsync(string artistId, bool includeTracks = false)
    {
        var artistReleaseResponse = await musicDataProvider.GetArtistReleasesByIdAsync(artistId, 0);

        var totalReleaseCount = artistReleaseResponse.RelseaseCount;
        var currentReleaseCount = artistReleaseResponse.Releases.Count();

        while (currentReleaseCount < totalReleaseCount && currentReleaseCount < musicDataOptions.MaxReleaseResults)
        {
            // Only make one request per second to avoid getting throttle by source api.
            // Will receive 'ServiceUnavailable' when exceeding more than one request per second
            Thread.Sleep(1000);
            var nextReleaseSet = await musicDataProvider.GetArtistReleasesByIdAsync(artistId, currentReleaseCount + 1);

            if (!nextReleaseSet.Releases.Any())
                break;

            artistReleaseResponse.Releases = artistReleaseResponse.Releases.Concat(nextReleaseSet.Releases);

            currentReleaseCount = artistReleaseResponse.Releases.Count();
        }

        // For perfomance and size reasons, not including all tracks when pull all releases
        // Allowing "includeTracks" for future tuning, but it will have no affect as of now
        return artistReleaseResponse.ToReleases(false);
    }

    public async Task<IEnumerable<ArtistRelease>> RetrieveAristReleasesAsync(string artistId, bool includeTracks = true)
    {
        logger.LogDebug("Retrieving releases for artist {Id}", artistId);

        var artistReleaseResponse = await musicDataProvider.GetArtistReleasesByIdAsync(artistId);

        return artistReleaseResponse.ToReleases(includeTracks);
    }

    public async Task<ArtistCollection> SearchArtistsAsync(string keyword, int page)
    {
        logger.LogDebug("Search artist {Keyword} on page {Page}", keyword, page);

        if (page < 1)
            throw new ArgumentException($"Page must be greater that zero; Current value: {page}", nameof(page));

        var limit = musicDataOptions.SearchArtistLimit;
        var offset = page == 1 ? 0 : (page - 1) * limit + 1;

        var actualSearchResults =
            await musicDataProvider.GetArtistsByKeywordAsync(keyword, limit, offset);

        var nextPage = offset + limit < actualSearchResults.Count ?
            string.Format(musicDataOptions.NextPageUrl, keyword, page + 1) : string.Empty;

        return new ArtistCollection
        {
            Total = actualSearchResults.Count,
            Next = nextPage,
            Artists = actualSearchResults.ToArtists()
        };
    }
}
