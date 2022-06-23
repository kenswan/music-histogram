using BlazorMusic.Shared;

namespace BlazorMusic.Client.Models;

public class ArtistReleasesViewModel
{
    public bool ArtistReleaseHasErrors { get; set; }

    public IEnumerable<ArtistRelease> FilteredReleases { get; set; }
}
