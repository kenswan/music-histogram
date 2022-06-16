using System.Text.Json.Serialization;

namespace BlazorMusic.Server.Models;

public class ArtistResponse
{
    public string Id { get; set; }
    public string Name { get; set; }

    [JsonPropertyName("disambiguation")]
    public string Description { get; set; }

    public string Type { get; set; }
    public int Score { get; set; }
    public string SortName { get; set; }
    public string Gender { get; set; }
    public string Country { get; set; }
    public IEnumerable<ArtistTagResponse> Tags { get; set; }
}
