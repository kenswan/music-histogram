using BlazorMusic.Shared;

namespace BlazorMusic.Server.Services;

public interface IArtistService
{
    Task<IEnumerable<ArtistRelease>> RetrieveAristReleasesAsync(string artistId);

    Task<ArtistCollection> SearchArtistsAsync(string keyword, int page);
}
