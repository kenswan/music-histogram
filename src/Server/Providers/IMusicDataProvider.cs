using BlazorMusic.Server.Models;

namespace BlazorMusic.Server.Providers;

public interface IMusicDataProvider
{
    Task<ArtistSearchResponse> GetArtistsByKeywordAsync(string keyword, int limit, int offset);
}
