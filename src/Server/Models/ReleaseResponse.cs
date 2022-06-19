using System.Text.Json.Serialization;

namespace BlazorMusic.Server.Models;

public class ReleaseResponse
{
    public string Id { get; set; }

    public string Country { get; set; }

    [JsonPropertyName("release-group")]
    public ReleaseGroupResponse ReleaseGroup { get; set; }

    public IEnumerable<MediaResponse> Media { get; set; }
}
