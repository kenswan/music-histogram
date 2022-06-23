using System.Text.Json.Serialization;

namespace BlazorMusic.Server.Models;

public class ArtistReleaseResponse
{
    [JsonPropertyName("release-count")]
    public int RelseaseCount { get; set; }

    [JsonPropertyName("release-offset")]
    public int RelseaseOffset { get; set; }

    public IEnumerable<ReleaseResponse> Releases { get; set; }
}
