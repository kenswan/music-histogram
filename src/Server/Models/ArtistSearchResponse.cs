namespace BlazorMusic.Server.Models;

public class ArtistSearchResponse
{
    public int Count { get; set; }
    public int Offset { get; set; }
    public IEnumerable<ArtistResponse> Artists { get; set; }
}
