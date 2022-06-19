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
}
