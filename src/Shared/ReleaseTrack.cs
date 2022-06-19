namespace BlazorMusic.Shared;
public class ReleaseTrack
{
    public string Id { get; set; }
    public string Title { get; set; }

    public int Position { get; set; }

    public long Duration { get; set; }

    public bool IsVideo { get; set; }

    public string ReleaseDate { get; set; }
}
