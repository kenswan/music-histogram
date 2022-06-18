using BlazorMusic.Shared;

namespace BlazorMusic.Client.Models;

public class ArtistStore
{
    public string CurrentSearchTerm = string.Empty;

    public string CurrentArtist { get; set; } = string.Empty;

    public IEnumerable<Artist> Artists { get; set; }
}
