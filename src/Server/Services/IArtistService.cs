using BlazorMusic.Shared;

namespace BlazorMusic.Server.Services;

/// <summary>
/// Service used to retrieve information pertaining to artists
/// </summary>
public interface IArtistService
{
    /// <summary>
    /// Retrieves artist releases by unique ID
    /// </summary>
    /// <param name="artistId">Artist ID</param>
    /// <returns>List of artist releases over time</returns>
    Task<IEnumerable<ArtistRelease>> RetrieveAristReleasesAsync(string artistId);

    /// <summary>
    /// Search artist by keyword
    /// </summary>
    /// <param name="keyword">Keyword search text</param>
    /// <param name="page">page of results to display (limit and offset combination)</param>
    /// <returns>List of artists matching search term</returns>
    Task<ArtistCollection> SearchArtistsAsync(string keyword, int page);
}
