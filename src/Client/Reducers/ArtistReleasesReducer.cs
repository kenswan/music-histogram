using BlazorFocused;
using BlazorMusic.Client.Models;

namespace BlazorMusic.Client.Reducers;

public class ArtistReleasesReducer : IReducer<ArtistStore, ArtistReleasesViewModel>
{
    public ArtistReleasesViewModel Execute(ArtistStore store)
    {
        ArtistReleasesViewModel data = new()
        {
            ArtistReleaseHasErrors = store.ReleaseError,

            FilteredReleases = store.Releases?.Where(release =>
                release.Country?.ToLower() == "us" &&
                release.Year > 0 &&
                release.TrackCount > 0 &&
                !string.IsNullOrEmpty(release.Format))
                // Remove duplicate titles shown by earliest appearance
                .GroupBy(release => release.Title).Select(release => release.OrderBy(release => release.Year).First())
                // Order total results by year
                .OrderByDescending(release => release.Year)
        };


        return data;
    }
}
