using BlazorFocused;
using BlazorMusic.Client.Models;

namespace BlazorMusic.Client.Reducers;

public class HistogramDataReducer : IReducer<ArtistStore, HistorgramData>
{
    public HistorgramData Execute(ArtistStore input)
    {
        Dictionary<int, int> counts = new();
        HistorgramData data = new();

        if (input.Releases is not null)
        {
            foreach (var release in input.Releases)
            {
                foreach (var track in release.Tracks)
                {
                    var hasKey = int.TryParse(track.ReleaseDate, out int key);
                    if (hasKey)
                    {
                        if (counts.ContainsKey(int.Parse(track.ReleaseDate)))
                            counts[key]++;
                        else
                            counts.Add(key, 1);
                    }
                }
            }

            data.Years = counts.Keys.ToArray();
            data.Releases = counts.Values.ToArray();
        }

        return data;
    }
}
