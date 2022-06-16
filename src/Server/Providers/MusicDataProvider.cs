using BlazorFocused;
using BlazorMusic.Server.Models;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Server.Providers;

public class MusicDataProvider : IMusicDataProvider
{
    private readonly MusicDataOptions musicDataOptions;
    private readonly IRestClient restClient;
    private readonly ILogger<MusicDataProvider> logger;

    public MusicDataProvider(IRestClient restClient, IOptions<MusicDataOptions> musicDataOptions, ILogger<MusicDataProvider> logger)
    {
        this.restClient = restClient;

        this.musicDataOptions = musicDataOptions.Value ??
            throw new ArgumentNullException(nameof(musicDataOptions), "Missing music data options. Check configuration");

        this.logger = logger;
    }

    public Task<ArtistSearchResponse> GetArtistsByKeywordAsync(string keyword, int limit, int offset)
    {
        logger.LogDebug("Get Artist - keyword: {Keyword}; limit:{limit}; offset: {Offset}", keyword, limit, offset);

        throw new NotImplementedException();
    }
}
