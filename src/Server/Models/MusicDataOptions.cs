namespace BlazorMusic.Server.Models;

public class MusicDataOptions
{
    public string SearchArtistUrl { get; set; }
    public int SearchArtistLimit { get; set; }
    public string NextPageUrl { get; set; }
    public string ArtistReleaseUrl { get; set; }
}
