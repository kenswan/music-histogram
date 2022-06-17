namespace BlazorMusic.Shared;

public class ArtistCollection
{
    public int Total { get; set; }
    public string Next { get; set; }
    public IEnumerable<Artist> Artists { get; set; }
}
