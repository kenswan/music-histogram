﻿using BlazorFocused;
using BlazorMusic.Client.Models;

namespace BlazorMusic.Client.Reducers;

public class HistogramDataReducer : IReducer<ArtistStore, HistorgramData>
{
    public HistorgramData Execute(ArtistStore input)
    {
        SortedDictionary<int, int> counts = new();
        HistorgramData data = new() { Years = Array.Empty<int>(), Releases = Array.Empty<int>() };

        if (input.Releases is not null)
        {
            foreach (var release in input.Releases)
            {
                var year = release.Year;
                var trackCount = release.TrackCount;

                // API returns 0 for years that were not identified
                if (release.Year > 0)
                {
                    if (counts.ContainsKey(year))
                        counts[year] += trackCount;
                    else
                        counts.Add(year, trackCount);
                }
            }

            data.Years = counts.Keys.ToArray();
            data.Releases = counts.Values.ToArray();
        }

        return data;
    }
}
