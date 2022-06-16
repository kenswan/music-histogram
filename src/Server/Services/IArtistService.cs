using BlazorMusic.Shared;

namespace BlazorMusic.Server.Services;

public interface IArtistService
{
    Task<ArtistCollection> SearchArtists(string keyword, int page);
}
