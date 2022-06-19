using BlazorMusic.Server.Models;
using Bogus;

namespace BlazorMusic.Server;

internal static partial class TestModels
{
    public static MusicDataOptions GenerateMusicDataOptions() =>
        new Faker<MusicDataOptions>()
        .RuleFor(options => options.SearchArtistUrl, _ => GenerateRandomRelativeUrl() + "?zero={0}&one={1}&two={2}")
        .RuleFor(options => options.SearchArtistLimit, _ => RandomCount)
        .RuleFor(options => options.NextPageUrl, _ => GenerateRandomRelativeUrl() + "?zero={0}&one={1}")
        .RuleFor(options => options.ArtistReleaseUrl, _ => GenerateRandomRelativeUrl() + "/{0}/test")
        .Generate();

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
