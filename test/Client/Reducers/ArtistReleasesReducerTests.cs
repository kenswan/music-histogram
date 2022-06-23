using BlazorMusic.Client.Models;
using BlazorMusic.Shared;

namespace BlazorMusic.Client.Reducers;

public class ArtistReleasesReducerTests
{
    private readonly ArtistReleasesReducer artistReleasesReducer;

    public ArtistReleasesReducerTests()
    {
        artistReleasesReducer = new();
    }

    [Theory]
    [InlineData("N/A", 2022, "CD", 13)]
    [InlineData("", 2022, "CD", 13)]
    [InlineData(null, 2022, "CD", 13)]
    [InlineData("US", 0, "CD", 13)]
    [InlineData("US", 2022, "CD", 0)]
    [InlineData("US", 2022, null, 13)]
    [InlineData("US", 2022, "", 13)]
    public void ShouldFilterReleases(string country, int year, string format, int trackCount)
    {
        var goodRecordId = TestModels.RandomIdentifier;
        var badRecordId = TestModels.RandomIdentifier;

        var releases = new List<ArtistRelease>
        {
            { new ArtistRelease { Id = goodRecordId, Title = "123", Country = "US", Year = 2022, Format = "CD", TrackCount = 15 } },
            { new ArtistRelease { Id = badRecordId, Title = "456", Country = country, Year = year, Format = format, TrackCount = trackCount } }
        };

        var store = new ArtistStore { Releases = releases, ReleaseError = false };

        var actualReleaseViewModel = artistReleasesReducer.Execute(store);

        Assert.Single(actualReleaseViewModel.FilteredReleases);
        Assert.Equal(goodRecordId, actualReleaseViewModel.FilteredReleases.First().Id);
    }

    [Fact]
    public void ShouldOrderByYearDescending()
    {
        var releases = new List<ArtistRelease>
        {
            { new ArtistRelease { Title = "123", Country = "US", Year = 2018, Format = "CD", TrackCount = 15 } },
            { new ArtistRelease { Title = "456",Country = "US", Year = 2020, Format = "CD", TrackCount = 15 } },
            { new ArtistRelease { Title = "789",Country = "US", Year = 2022, Format = "CD", TrackCount = 15 } },
            { new ArtistRelease { Title = "098",Country = "US", Year = 2019, Format = "CD", TrackCount = 15 } }
        };

        var store = new ArtistStore { Releases = releases, ReleaseError = false };

        var actualReleaseYears = artistReleasesReducer.Execute(store).FilteredReleases.Select(release => release.Year).ToArray();

        Assert.Equal(2022, actualReleaseYears[0]);
        Assert.Equal(2020, actualReleaseYears[1]);
        Assert.Equal(2019, actualReleaseYears[2]);
        Assert.Equal(2018, actualReleaseYears[3]);
    }

    [Fact]
    public void ShouldRemoveDuplicateTitlesByUsingEarliestYear()
    {
        var expectedId = TestModels.RandomIdentifier;
        var excludedId = TestModels.RandomIdentifier;

        var releases = new List<ArtistRelease>
        {
            { new ArtistRelease { Id = excludedId, Title = "123",Country = "US", Year = 2020, Format = "CD", TrackCount = 15 } },
            { new ArtistRelease { Id = expectedId, Title = "123", Country = "US", Year = 2018, Format = "CD", TrackCount = 15 } }
        };

        var store = new ArtistStore { Releases = releases, ReleaseError = false };

        var actualReleases = artistReleasesReducer.Execute(store).FilteredReleases;

        Assert.Single(actualReleases);
        Assert.Equal(expectedId, actualReleases.First().Id);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldReturnReleaseErrorFlag(bool hasReleaseError)
    {
        var expectedId = TestModels.RandomIdentifier;
        var excludedId = TestModels.RandomIdentifier;

        var store = new ArtistStore { Releases = Enumerable.Empty<ArtistRelease>(), ReleaseError = hasReleaseError };

        Assert.Equal(hasReleaseError, artistReleasesReducer.Execute(store).ArtistReleaseHasErrors);
    }
}
