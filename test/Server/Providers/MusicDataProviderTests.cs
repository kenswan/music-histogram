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

        restClientMock.Setup(client => client.GetAsync<ArtistSearchResponse>(expectedUrl))
            .ReturnsAsync(expectedSearchResults);

        var actualSearchResults =
            await musicDataProvider.GetArtistsByKeywordAsync(keyword, limit, offset);

        actualSearchResults.Should().BeEquivalentTo(expectedSearchResults);
    }
}
