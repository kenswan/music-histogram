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

    public record ArtistComparison(string Id, string Name, string Description, string Country, string Type);

    public record ReleaseComparison(string Id, string Title, string Media, int TrackCount, int ActualCount);

    [Theory]
    [InlineData(100, 1, 0)] // page 1
    [InlineData(100, 2, 101)] // page 2
    [InlineData(100, 3, 201)] // page 3
    [InlineData(50, 3, 101)] // page 3, lower limit
    public async Task ShouldSearchArtistByPage(int limit, int page, int expectedOffset)
    {
        var searchKeyword = TestModels.RandomString;
        var searchResults = TestModels.GenerateArtistSearchResponse();
        var expectedArtists = searchResults.Artists.Select(artist => GetArtistComparison(artist));

        musicDataOptions.SearchArtistLimit = limit;

        musicDataProviderMock.Setup(provider =>
            provider.GetArtistsByKeywordAsync(searchKeyword, limit, expectedOffset))
                .ReturnsAsync(searchResults);

        var actualSearchResults = await artistService.SearchArtistsAsync(searchKeyword, page);

        var actualArtists = actualSearchResults.Artists.Select(artist => GetArtistComparison(artist));

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

        var actualSearchResults = await artistService.SearchArtistsAsync(searchKeyword, page);

        Assert.Equal(expectedNextPageUrl, actualSearchResults.Next);
    }

    [Fact]
    public async Task ShouldThrowIfPageIsLessThanOne()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            artistService.SearchArtistsAsync(TestModels.RandomString, 0));
    }

    [Fact]
    public async Task ShouldRetrieveArtistReleases()
    {
        var artistId = TestModels.RandomIdentifier;
        var artistReleaseResponse = TestModels.GenerateArtistReleaseResponse();

        IEnumerable<ReleaseComparison> expectedReleases =
            artistReleaseResponse.Releases.Select(release => GetReleaseComparison(release));

        musicDataProviderMock.Setup(provider =>
            provider.GetArtistReleasesByIdAsync(artistId))
                .ReturnsAsync(artistReleaseResponse);

        var actualArtistReleases = await artistService.RetrieveAristReleasesAsync(artistId);

        var actualReleases = actualArtistReleases.Select(release => GetReleaseComparison(release));

        actualReleases.Should().BeEquivalentTo(expectedReleases);
    }

    [Fact]
    public async Task ShouldRetrieveAllArtistReleases()
    {
        var artistId = TestModels.RandomIdentifier;
        var firstReleaseSet = TestModels.GenerateArtistReleaseResponse(2);
        var secondReleaseSet = TestModels.GenerateArtistReleaseResponse(3);
        var thirdReleaseSet = TestModels.GenerateArtistReleaseResponse(4);

        var totalReleaseSetCount = 9;
        firstReleaseSet.RelseaseCount = totalReleaseSetCount;
        secondReleaseSet.RelseaseCount = totalReleaseSetCount;
        thirdReleaseSet.RelseaseCount = totalReleaseSetCount;

        var expectedFirstReleaseSet = firstReleaseSet.ToReleases(false);
        var expectedSecondReleaseSet = secondReleaseSet.ToReleases(false);
        var expectedThirdReleaseSet = thirdReleaseSet.ToReleases(false);

        var expectedOverall =
            expectedFirstReleaseSet.Concat(expectedSecondReleaseSet).Concat(expectedThirdReleaseSet);

        musicDataProviderMock.Setup(provider =>
            provider.GetArtistReleasesByIdAsync(artistId, 0))
                .ReturnsAsync(firstReleaseSet);

        musicDataProviderMock.Setup(provider =>
            provider.GetArtistReleasesByIdAsync(artistId, 3))
                .ReturnsAsync(secondReleaseSet);

        musicDataProviderMock.Setup(provider =>
            provider.GetArtistReleasesByIdAsync(artistId, 6))
                .ReturnsAsync(thirdReleaseSet);

        var actualArtistReleases = await artistService.RetrieveAllAristReleasesAsync(artistId, false);

        Assert.Equal(totalReleaseSetCount, actualArtistReleases.Count());

        actualArtistReleases.Should().BeEquivalentTo(expectedOverall);
    }

    private static ReleaseComparison GetReleaseComparison(ReleaseResponse releaseResponse) =>
        new(
            Id: releaseResponse.Id,
            Title: releaseResponse.ReleaseGroup.Title,
            Media: releaseResponse.ReleaseGroup.Type,
            TrackCount: releaseResponse.Media.First().TrackCount,
            ActualCount: releaseResponse.Media.First().Tracks.Count()
        );

    private static ReleaseComparison GetReleaseComparison(ArtistRelease artistRelease) =>
        new(
            Id: artistRelease.Id,
            Title: artistRelease.Title,
            Media: artistRelease.MediaType,
            TrackCount: artistRelease.TrackCount,
            ActualCount: artistRelease.Tracks.Count()
        );

    private static ArtistComparison GetArtistComparison(ArtistResponse artistResponse) =>
        new(
        Id: artistResponse.Id,
        Name: artistResponse.Name,
        Description: artistResponse.Description,
        Country: artistResponse.Country,
        Type: artistResponse.Type);

    private static ArtistComparison GetArtistComparison(Artist artist) =>
        new(
        Id: artist.Id,
        Name: artist.Name,
        Description: artist.Description,
        Country: artist.Country,
        Type: artist.Type);
}
