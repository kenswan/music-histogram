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

    private static Faker<Artist> GenerateArtistFake() =>
        new Faker<Artist>()
        .RuleFor(response => response.Id, _ => RandomIdentifier)
        .RuleFor(response => response.Name, fake => fake.Person.FullName)
        .RuleFor(response => response.Description, fake => fake.Lorem.Sentences(2))
        .RuleFor(response => response.Country, fake => fake.Address.CountryCode())
        .RuleFor(response => response.Tags, fake => fake.Make(RandomCount, _ => fake.Random.Word()));

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
