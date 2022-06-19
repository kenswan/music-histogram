using BlazorMusic.Server.Models;
using Bogus;

namespace BlazorMusic.Server;
internal static partial class TestModels
{
    public static ArtistReleaseResponse GenerateArtistReleaseResponse() =>
        new Faker<ArtistReleaseResponse>()
        .RuleFor(artistRelease => artistRelease.Releases, _ => GenerateReleaseResponses())
        .Generate();

    public static IEnumerable<ReleaseResponse> GenerateReleaseResponses() =>
        new Faker<ReleaseResponse>()
        .RuleFor(release => release.Id, _ => RandomIdentifier)
        .RuleFor(release => release.Country, fake => fake.Address.Country())
        .RuleFor(release => release.Media, _ => GenerateMediaResponse())
        .RuleFor(release => release.ReleaseGroup, _ => GenerateReleaseGroupResponse())
        .Generate(RandomCount);

    public static ReleaseGroupResponse GenerateReleaseGroupResponse() =>
        new Faker<ReleaseGroupResponse>()
        .RuleFor(group => group.ReleaseDate, fake => fake.Date.Past().Year.ToString())
        .RuleFor(group => group.Title, fake => fake.Lorem.Sentence(RandomCount))
        .RuleFor(group => group.Type, fake => fake.PickRandom<string>("Album", "EP", "Single"))
        .Generate();

    public static IEnumerable<MediaResponse> GenerateMediaResponse() =>
        new Faker<MediaResponse>()
        .RuleFor(media => media.TrackCount, _ => RandomCount)
        .RuleFor(media => media.Format, fake => fake.PickRandom<string>("CD", "Vinyl", "Digital"))
        .RuleFor(media => media.Title, fake => fake.Lorem.Sentence(RandomCount))
        .RuleFor(media => media.Tracks, _ => GenerateTrackResponses())
        .Generate(1);

    public static IEnumerable<TrackResponse> GenerateTrackResponses() =>
        new Faker<TrackResponse>()
        .RuleFor(track => track.Duration, fake => fake.Random.Long())
        .RuleFor(track => track.Title, fake => fake.Lorem.Sentence(RandomCount))
        .RuleFor(track => track.Position, fake => fake.Random.Int())
        .RuleFor(track => track.Recording, _ => GenerateRecordingResponse())
        .Generate(RandomCount);

    public static RecordingResponse GenerateRecordingResponse() =>
        new Faker<RecordingResponse>()
        .RuleFor(recording => recording.Id, _ => RandomIdentifier)
        .RuleFor(recording => recording.IsVideo, fake => fake.PickRandom(true, false))
        .RuleFor(recording => recording.Title, fake => fake.Lorem.Sentence(RandomCount))
        .RuleFor(recording => recording.ReleaseDate, fake => fake.Date.Past().Year.ToString())
        .Generate();
}
