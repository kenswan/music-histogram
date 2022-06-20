using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using FluentAssertions;

namespace BlazorMusic.Client.Reducers;
public class HistogramDataReducerTests
{
    private readonly HistogramDataReducer histogramDataReducer;
    public HistogramDataReducerTests()
    {
        histogramDataReducer = new();
    }

    [Fact]
    public void ShouldConsolidateYearsAndReleaseCounts()
    {
        var releases = new List<ArtistRelease>
        {
            { new ArtistRelease { Year = 2022, TrackCount = 15 } },
            { new ArtistRelease { Year = 2021, TrackCount = 1 } },
            { new ArtistRelease { Year = 2022, TrackCount = 30 } },
            { new ArtistRelease { Year = 2020, TrackCount = 5 } },
            { new ArtistRelease { Year = 2022, TrackCount = 3 } }
        };

        var expectedYears = new int[] { 2020, 2021, 2022 };
        var expectedReleaseCounts = new int[] { 5, 1, 48 };

        var artistStore = new ArtistStore { Releases = releases };

        var actualHistogramData = histogramDataReducer.Execute(artistStore);

        actualHistogramData.Years.Should().BeEquivalentTo(expectedYears);
        actualHistogramData.Releases.Should().BeEquivalentTo(expectedReleaseCounts);
    }

    [Fact]
    public void ShouldReturnEmptyWhenNoReleasesPresent()
    {
        var artistStore = new ArtistStore { Releases = null };

        var actualHistogramData = histogramDataReducer.Execute(artistStore);

        actualHistogramData.Years.Should().BeEmpty();
        actualHistogramData.Releases.Should().BeEmpty();
    }
}
