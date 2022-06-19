using System.Text.Json.Serialization;

namespace BlazorMusic.Server.Models;

public class RecordingResponse
{
    public string Id { get; set; }

    public string Title { get; set; }

    [JsonPropertyName("length")]
    public long? Duration { get; set; }

    [JsonPropertyName("video")]
    public bool IsVideo { get; set; }

    [JsonPropertyName("first-release-date")]
    public string ReleaseDate { get; set; }
}
