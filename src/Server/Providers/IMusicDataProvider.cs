using BlazorMusic.Server.Models;

namespace BlazorMusic.Server.Providers;

public interface IMusicDataProvider
{
    Task<ArtistReleaseResponse> GetArtistReleasesByIdAsync(string artistId);

    Task<ArtistSearchResponse> GetArtistsByKeywordAsync(string keyword, int limit, int offset);
}
