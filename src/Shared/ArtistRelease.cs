namespace BlazorMusic.Shared;

public class ArtistRelease
{
    public string Id { get; set; }

    public string Country { get; set; }

    public string MediaType { get; set; }

    public string Format { get; set; }

    public string Title { get; set; }

    public int TrackCount { get; set; }

    public IEnumerable<ReleaseTrack> Tracks { get; set; }
}
