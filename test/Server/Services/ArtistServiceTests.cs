using BlazorMusic.Server.Extensions;
using BlazorMusic.Server.Models;
using BlazorMusic.Server.Providers;
using BlazorMusic.Shared;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BlazorMusic.Server.Services;

public class ArtistServiceTests
{
    private readonly Mock<IMusicDataProvider> musicDataProviderMock;
    private readonly MusicDataOptions musicDataOptions;

    private readonly IArtistService artistService;

    public ArtistServiceTests()
    {
        musicDataProviderMock = new();
        musicDataOptions = TestModels.GenerateMusicDataOptions();

        artistService = new ArtistService(
            musicDataProviderMock.Object,
            Options.Create(musicDataOptions),
            NullLogger<ArtistService>.Instance);
    }

    [Theory]
    [InlineData(100, 1, 0)] // page 1
    [InlineData(100, 2, 101)] // page 2
    [InlineData(100, 3, 201)] // page 3
    [InlineData(50, 3, 101)] // page 3, lower limit
    public async Task ShouldSearchArtistByPage(int limit, int page, int expectedOffset)
    {
        var searchKeyword = TestModels.RandomString;
        var searchResults = TestModels.GenerateArtistSearchResponse();
        IEnumerable<Artist> expectedArtists = searchResults.ToArtists();
        musicDataOptions.SearchArtistLimit = limit;

        musicDataProviderMock.Setup(provider =>
            provider.GetArtistsByKeywordAsync(searchKeyword, limit, expectedOffset))
                .ReturnsAsync(searchResults);

        var actualSearchResults = await artistService.SearchArtists(searchKeyword, page);

        actualSearchResults.Artists.Should().BeEquivalentTo(expectedArtists);
    }

    [Theory]
    [InlineData(100, 50, 1, true)]
    [InlineData(100, 10, 2, true)]
    [InlineData(10, 10, 1, false)]
    [InlineData(5, 10, 1, false)]
    public async Task ShouldReturnNextPage(int total, int limit, int page, bool hasNextPage)
    {
        var searchKeyword = TestModels.RandomString;
        var searchResults = TestModels.GenerateArtistSearchResponse();
        searchResults.Count = total;
        musicDataOptions.SearchArtistLimit = limit;

        var expectedNextPageUrl = hasNextPage ?
            string.Format(musicDataOptions.NextPageUrl, searchKeyword, page + 1) : string.Empty;

        musicDataProviderMock.Setup(provider =>
            provider.GetArtistsByKeywordAsync(searchKeyword, limit, It.IsAny<int>())) // offset is tested above
                .ReturnsAsync(searchResults);

        var actualSearchResults = await artistService.SearchArtists(searchKeyword, page);

        Assert.Equal(expectedNextPageUrl, actualSearchResults.Next);
    }

    [Fact]
    public async Task ShouldThrowIfPageIsLessThanOne()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            artistService.SearchArtists(TestModels.RandomString, 0));
    }
}
