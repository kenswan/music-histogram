using BlazorMusic.Server.Models;
using Bogus;

namespace BlazorMusic.Server;

internal static class TestModels
{
    public static ArtistSearchResponse GenerateArtistSearchResponse() =>
        new Faker<ArtistSearchResponse>()
        .RuleFor(response => response.Count, _ => RandomInteger)
        .RuleFor(response => response.Offset, _ => RandomInteger)
        .RuleFor(response => response.Artists, _ => GenerateArtistResponses())
        .Generate();

    public static IEnumerable<ArtistResponse> GenerateArtistResponses() =>
        GenerateArtistResponseFake()
        .Generate(RandomCount);

    public static ArtistResponse GenerateArtistResponse() =>
        GenerateArtistResponseFake()
        .Generate();

    public static IEnumerable<ArtistTagResponse> GenerateArtistTagResponses() =>
        new Faker<ArtistTagResponse>()
        .RuleFor(tag => tag.Count, _ => RandomCount)
        .RuleFor(tag => tag.Name, fake => fake.Music.Genre())
        .Generate(RandomCount);

    public static MusicDataOptions GenerateMusicDataOptions() =>
        new Faker<MusicDataOptions>()
        .RuleFor(options => options.SearchArtistUrl, _ => GenerateRandomRelativeUrl() + "?zero={0}&one={1}&two={2}")
        .RuleFor(options => options.SearchArtistLimit, _ => RandomCount)
        .RuleFor(options => options.NextPageUrl, _ => GenerateRandomRelativeUrl() + "?zero={0}&one={1}")
        .Generate();

    public static string GenerateRandomRelativeUrl() =>
        new Faker().Internet.UrlRootedPath();

    private static Faker<ArtistResponse> GenerateArtistResponseFake() =>
        new Faker<ArtistResponse>()
        .RuleFor(response => response.Id, _ => RandomIdentifier)
        .RuleFor(response => response.Name, fake => fake.Person.FullName)
        .RuleFor(response => response.Description, fake => fake.Lorem.Sentences(2))
        .RuleFor(response => response.SortName, fake => $"{fake.Person.LastName}, {fake.Person.FirstName}")
        .RuleFor(response => response.Country, fake => fake.Address.CountryCode())
        .RuleFor(response => response.Tags, _ => GenerateArtistTagResponses())
        .RuleFor(response => response.Gender, fake => fake.Person.Gender.ToString());

    public static string RandomIdentifier =>
        new Faker().Random.Guid().ToString();

    public static int RandomInteger =>
        new Faker().Random.Int(2, 10000);

    public static int RandomCount =>
        new Faker().Random.Int(2, 5);

    public static string RandomString =>
        new Faker().Random.String2(RandomCount);
}
