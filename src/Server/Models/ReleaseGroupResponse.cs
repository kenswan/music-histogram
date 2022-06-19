using System.Text.Json.Serialization;

namespace BlazorMusic.Server.Models;

public class ReleaseGroupResponse
{
    public string Id { get; set; }

    public string Title { get; set; }

    [JsonPropertyName("primary-type")]
    public string Type { get; set; }

    [JsonPropertyName("first-release-date")]
    public string ReleaseDate { get; set; }
}
