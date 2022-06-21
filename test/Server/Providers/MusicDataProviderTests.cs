using BlazorFocused;
using BlazorMusic.Server.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BlazorMusic.Server.Providers;

public class MusicDataProviderTests
{
    private readonly Mock<IRestClient> restClientMock;
    private readonly MusicDataOptions musicDataOptions;

    private readonly IMusicDataProvider musicDataProvider;

    public MusicDataProviderTests()
    {
        restClientMock = new();
        musicDataOptions = TestModels.GenerateMusicDataOptions();

        musicDataProvider = new MusicDataProvider(
            restClientMock.Object,
            Options.Create(musicDataOptions),
            NullLogger<MusicDataProvider>.Instance);
    }

    [Fact]
    public async Task ShouldGetArtistsBySearchAndLimit()
    {
        var limit = TestModels.RandomInteger;
        var offset = TestModels.RandomInteger;
        var keyword = TestModels.RandomString;
        var relativeUrl = musicDataOptions.SearchArtistUrl;
        var expectedUrl = string.Format(relativeUrl, keyword, limit, offset);
        var expectedSearchResults = TestModels.GenerateArtistSearchResponse();

        restClientMock.Setup(client =>
            client.GetAsync<ArtistSearchResponse>(expectedUrl))
                .ReturnsAsync(expectedSearchResults);

        var actualSearchResults =
            await musicDataProvider.GetArtistsByKeywordAsync(keyword, limit, offset);

        actualSearchResults.Should().BeEquivalentTo(expectedSearchResults);
    }

    [Fact]
    public async Task ShouldGetArtistsReleases()
    {
        var artistId = TestModels.RandomString;
        var relativeUrl = musicDataOptions.ArtistReleaseUrl;
        var expectedUrl = string.Format(relativeUrl, artistId);
        var expectedArtistReleaseResponse = TestModels.GenerateArtistReleaseResponse();

        restClientMock.Setup(client =>
            client.GetAsync<ArtistReleaseResponse>(expectedUrl))
                .ReturnsAsync(expectedArtistReleaseResponse);

        var actualActistReleaseResponse =
            await musicDataProvider.GetArtistReleasesByIdAsync(artistId);

        actualActistReleaseResponse.Should().BeEquivalentTo(expectedArtistReleaseResponse);
    }

    [Fact]
    public async Task ShouldGetArtistsReleasesWithOffsetValue()
    {
        var artistId = TestModels.RandomString;
        var offset = TestModels.RandomInteger;
        var relativeUrl = musicDataOptions.MaxArtistReleaseUrl;
        var expectedUrl = string.Format(relativeUrl, artistId, offset);
        var expectedArtistReleaseResponse = TestModels.GenerateArtistReleaseResponse();

        restClientMock.Setup(client =>
            client.GetAsync<ArtistReleaseResponse>(expectedUrl))
                .ReturnsAsync(expectedArtistReleaseResponse);

        var actualActistReleaseResponse =
            await musicDataProvider.GetArtistReleasesByIdAsync(artistId, offset);

        actualActistReleaseResponse.Should().BeEquivalentTo(expectedArtistReleaseResponse);
    }
}
