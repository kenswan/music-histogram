using BlazorMusic.Server.Models;
using BlazorMusic.Shared;

namespace BlazorMusic.Server.Extensions;

public static class ArtistSearchResponseExtensions
{
    public static IEnumerable<Artist> ToArtists(this ArtistSearchResponse artistResponse) =>
        artistResponse.Artists.Select(response =>
            new Artist
            {
                Id = response.Id,
                Name = response.Name,
                Description = response.Description,
                Country = response?.Country,
                Type = response?.Type,
                Rank = response.Score,
                Tags = response.Tags?.Select(tag => tag.Name)
            });

    public static IEnumerable<ArtistRelease> ToReleases(this ArtistReleaseResponse artistReleaseResponse) =>
        artistReleaseResponse.Releases.Select(release =>
            new ArtistRelease
            {
                Id = release.Id,
                Title = release.ReleaseGroup.Title,
                Country = release.Country,
                Year = FormatYear(release.Date),
                MediaType = release.ReleaseGroup.Type,
                Format = release.Media.First().Format,
                TrackCount = release.Media.First().TrackCount,
                Tracks = release.Media.First().Tracks.ToTracks()
            });

    public static IEnumerable<ReleaseTrack> ToTracks(this IEnumerable<TrackResponse> trackResponses) =>
        trackResponses.Select(track =>
            new ReleaseTrack
            {
                Id = track.Id,
                Title = track.Title,
                Position = track.Position,
                ReleaseDate = track.Recording.ReleaseDate,
                Duration = track.Duration ?? 0,
                IsVideo = track.Recording.IsVideo
            });

    private static int FormatYear(string date)
    {
        int year = 0;

        if (!string.IsNullOrEmpty(date) && date.Length >= 4)
        {
            if (int.TryParse(date[..4], out int frontYear))
                year = frontYear;
            else if (date.Length > 4 && int.TryParse(date.AsSpan(date.Length - 4, 4), out int backYear))
                year = backYear;
        }

        return year;
    }
}
