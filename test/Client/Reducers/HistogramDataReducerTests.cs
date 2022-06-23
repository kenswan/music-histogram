using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using FluentAssertions;

namespace BlazorMusic.Client.Reducers;
public class HistogramDataReducerTests
{
    private readonly ArtistHistogramReducer histogramDataReducer;
    public HistogramDataReducerTests()
    {
        histogramDataReducer = new();
    }

    [Fact]
    public void ShouldConsolidateYearsAndReleaseCounts()
    {
        var currentArtist = TestModels.GenerateArtist();

        var releases = new List<ArtistRelease>
        {
            { new ArtistRelease { Year = 2022, TrackCount = 15 } },
            { new ArtistRelease { Year = 2021, TrackCount = 1 } },
            { new ArtistRelease { Year = 2022, TrackCount = 30 } },
            { new ArtistRelease { Year = 2020, TrackCount = 5 } },
            { new ArtistRelease { Year = 2022, TrackCount = 3 } }
        };

        var expectedHistogramData = new ArtistHistorgramViewModel
        {
            Years = new int[] { 2020, 2021, 2022 },
            Releases = new int[] { 5, 1, 48 },
            CsvFormat = "2020,5\n2021,1\n2022,48",
            ArtistName = currentArtist.Name
        };

        var artistStore = new ArtistStore { Releases = releases, CurrentArtist = currentArtist };

        var actualHistogramData = histogramDataReducer.Execute(artistStore);

        actualHistogramData.Should().BeEquivalentTo(expectedHistogramData);
    }

    [Fact]
    public void ShouldReturnEmptyWhenNoReleasesPresent()
    {
        var artistStore = new ArtistStore { Releases = null };

        var actualHistogramData = histogramDataReducer.Execute(artistStore);

        actualHistogramData.Years.Should().BeEmpty();
        actualHistogramData.Releases.Should().BeEmpty();
        actualHistogramData.ArtistName.Should().BeEmpty();
        actualHistogramData.CsvFormat.Should().BeEmpty();
    }
}
