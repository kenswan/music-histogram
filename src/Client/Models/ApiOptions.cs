namespace BlazorMusic.Client.Models;

public class ApiOptions
{
    public string ArtistReleaseUrl { get; set; }

    public string ArtistMaxReleaseUrl { get; set; }

    public string SearchArtistUrl { get; set; }

    public string ReleaseTracksUrl { get; set; }

    public bool MaxReleaseEnabled { get; set; }
}
