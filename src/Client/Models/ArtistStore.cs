using BlazorMusic.Shared;

namespace BlazorMusic.Client.Models;

public class ArtistStore
{
    public string CurrentSearchTerm = string.Empty;

    public Artist CurrentArtist { get; set; }

    public IEnumerable<Artist> Artists { get; set; }
}
