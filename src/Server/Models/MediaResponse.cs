using System.Text.Json.Serialization;

namespace BlazorMusic.Server.Models;

public class MediaResponse
{
    public string Format { get; set; }

    public string Title { get; set; }

    [JsonPropertyName("track-count")]
    public int TrackCount { get; set; }

    public IEnumerable<TrackResponse> Tracks { get; set; }
}
