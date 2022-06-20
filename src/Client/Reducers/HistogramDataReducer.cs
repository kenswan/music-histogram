using BlazorFocused;
using BlazorMusic.Client.Models;

namespace BlazorMusic.Client.Reducers;

public class HistogramDataReducer : IReducer<ArtistStore, HistorgramData>
{
    public HistorgramData Execute(ArtistStore input)
    {
        SortedDictionary<int, int> releaseCounts = new();

        HistorgramData data = new()
        {
            Years = Array.Empty<int>(),
            Releases = Array.Empty<int>(),
            ArtistName = string.Empty,
            CsvFormat = string.Empty
        };

        if (input.Releases is not null)
        {
            foreach (var release in input.Releases)
            {
                var year = release.Year;
                var trackCount = release.TrackCount;

                // API returns 0 for years that were not identified
                if (release.Year > 0)
                {
                    if (releaseCounts.ContainsKey(year))
                        releaseCounts[year] += trackCount;
                    else
                        releaseCounts.Add(year, trackCount);
                }
            }

            data.Years = releaseCounts.Keys.ToArray();
            data.Releases = releaseCounts.Values.ToArray();
            data.ArtistName = input.CurrentArtist?.Name;

            data.CsvFormat = string.Join("\n",
                (releaseCounts.Select(releaseCount => $"{releaseCount.Key},{releaseCount.Value}")));

        }

        return data;
    }
}
