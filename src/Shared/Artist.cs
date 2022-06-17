namespace BlazorMusic.Shared;

public class Artist
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Country { get; set; }
    public int Rank { get; set; }
    public string Type { get; set; }
    public IEnumerable<string> Tags { get; set; }
}
