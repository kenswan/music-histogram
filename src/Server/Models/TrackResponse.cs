using System.Text.Json.Serialization;

namespace BlazorMusic.Server.Models;

public class TrackResponse
{
    public string Id { get; set; }

    public string Title { get; set; }

    public int Position { get; set; }

    [JsonPropertyName("length")]
    public long? Duration { get; set; }

    public RecordingResponse Recording { get; set; }
}
