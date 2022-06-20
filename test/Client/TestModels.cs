using BlazorMusic.Shared;
using Bogus;

namespace BlazorMusic.Client;
public static class TestModels
{
    public static ArtistCollection GenerateArtistCollection() =>
        new Faker<ArtistCollection>()
        .RuleFor(collection => collection.Next, _ => RandomIdentifier)
        .RuleFor(collection => collection.Total, _ => RandomInteger)
        .RuleFor(collection => collection.Artists, _ => GenerateArtists())
        .Generate();

    public static Artist GenerateArtist() =>
        GenerateArtistFake().Generate();

    public static IEnumerable<Artist> GenerateArtists() =>
        GenerateArtistFake().Generate(RandomCount);

    public static IEnumerable<ArtistRelease> GenerateArtistReleases() =>
        new Faker<ArtistRelease>()
        .RuleFor(release => release.Id, _ => RandomIdentifier)
        .RuleFor(release => release.Year, fake => fake.Date.Past().Year)
        .RuleFor(release => release.Title, fake => fake.Lorem.Sentences(1))
        .RuleFor(release => release.Country, fake => fake.Address.CountryCode())
        .RuleFor(release => release.Tracks, _ => GenerateReleaseTracks())
        .RuleFor(release => release.TrackCount, fake => fake.Random.Int())
        .Generate(RandomCount);

    public static IEnumerable<ReleaseTrack> GenerateReleaseTracks() =>
        new Faker<ReleaseTrack>()
        .RuleFor(release => release.Id, _ => RandomIdentifier)
        .RuleFor(release => release.Title, fake => fake.Lorem.Sentences(1))
        .RuleFor(release => release.Duration, fake => fake.Random.Long(1000000, 999999))
        .Generate(RandomCount);

    private static Faker<Artist> GenerateArtistFake() =>
        new Faker<Artist>()
        .RuleFor(artist => artist.Id, _ => RandomIdentifier)
        .RuleFor(artist => artist.Name, fake => fake.Person.FullName)
        .RuleFor(artist => artist.Description, fake => fake.Lorem.Sentences(2))
        .RuleFor(artist => artist.Country, fake => fake.Address.CountryCode())
        .RuleFor(artist => artist.Tags, fake => fake.Make(RandomCount, _ => fake.Random.Word()));

    public static string GenerateRandomRelativeUrl() =>
        new Faker().Internet.UrlRootedPath();
    public static string RandomIdentifier =>
        new Faker().Random.Guid().ToString();

    public static int RandomInteger =>
        new Faker().Random.Int(2, 10000);

    public static int RandomCount =>
        new Faker().Random.Int(2, 5);

    public static string RandomString =>
        new Faker().Random.String2(RandomCount);
}
