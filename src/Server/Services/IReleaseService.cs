using BlazorMusic.Shared;

namespace BlazorMusic.Server.Services;

public interface IReleaseService
{
    /// <summary>
    /// Retrieves list of tracks associated to a given release
    /// </summary>
    /// <param name="id">Unique identifier of release</param>
    /// <returns>List of tracks for a given release</returns>
    Task<IEnumerable<ReleaseTrack>> RetrieveReleaseTracksByIdAsync(string id);
}
