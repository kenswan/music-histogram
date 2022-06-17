using BlazorMusic.Server.Models;
using BlazorMusic.Shared;

namespace BlazorMusic.Server.Extensions;

public static class ArtistSearchResponseExtensions
{
    public static IEnumerable<Artist> ToArtists(this ArtistSearchResponse artistResponse)
    {
        return artistResponse.Artists.Select(response =>
            new Artist
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                Country = response.Country,
                Tags = response.Tags.Select(tag => tag.Name)
            });
    }
}
