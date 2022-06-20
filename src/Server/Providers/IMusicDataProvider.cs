using BlazorMusic.Server.Models;

namespace BlazorMusic.Server.Providers;

/// <summary>
/// Provider used to retrieve information from music source api
/// </summary>
public interface IMusicDataProvider
{
    /// <summary>
    /// Get releases information for a given artist
    /// </summary>
    /// <param name="artistId">Unique artist identifier</param>
    /// <returns>List of releases for artist</returns>
    Task<ArtistReleaseResponse> GetArtistReleasesByIdAsync(string artistId);

    /// <summary>
    /// Get collection of artists matching a keyword
    /// </summary>
    /// <param name="keyword">Keyword search text</param>
    /// <param name="limit">Limits amount of results returned</param>
    /// <param name="offset"></param>
    /// <returns>List of artists matching search term</returns>
    Task<ArtistSearchResponse> GetArtistsByKeywordAsync(string keyword, int limit, int offset);
}
