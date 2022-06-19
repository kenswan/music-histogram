namespace BlazorMusic.Server.Models;

public class ArtistReleaseResponse
{
    public IEnumerable<ReleaseResponse> Releases { get; set; }
}
