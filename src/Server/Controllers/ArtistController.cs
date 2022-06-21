using BlazorMusic.Server.Services;
using BlazorMusic.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace BlazorMusic.Server.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class ArtistController : ControllerBase
{
    private readonly IArtistService artistService;
    private readonly ILogger<ArtistController> logger;

    public ArtistController(IArtistService artistService, ILogger<ArtistController> logger)
    {
        this.artistService = artistService;
        this.logger = logger;
    }

    /// <summary>
    /// Performs a search for an artist with a given keyword
    /// </summary>
    /// <param name="search">Keyword search text</param>
    /// <param name="page">Page # of intended result (searches with offset from previous page)</param>
    /// <returns>List of artists matching search criteria</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ArtistCollection), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ArtistCollection>> SearchArtist([FromQuery][Required] string search, [FromQuery] int page = 1)
    {
        logger.LogInformation("Request Artist Search Keyword: {Search}", search);

        var artistCollection = await artistService.SearchArtistsAsync(search, page);

        if (artistCollection is null || !artistCollection.Artists.Any())
            return NotFound();

        return Ok(artistCollection);
    }

    /// <summary>
    /// Retrieves all releases for a given artist
    /// </summary>
    /// <param name="id">Unique identifier of artist</param>
    /// <returns>List of artist releases over time</returns>
    [HttpGet("{id}/release")]
    [ProducesResponseType(typeof(IEnumerable<ArtistRelease>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<ArtistRelease>>> GetArtistReleases(
        [FromRoute] string id,
        [FromQuery] bool includeTracks = false,
        [FromQuery] bool includeAllReleases = false)
    {
        logger.LogInformation("Request Releases for Artist {Id}", id);

        var artistReleases = includeAllReleases ?
            await artistService.RetrieveAllAristReleasesAsync(id) : // For perfomance and size reasons, not including all tracks when pull all releases
            await artistService.RetrieveAristReleasesAsync(id, includeTracks);

        if (artistReleases is null || !artistReleases.Any())
            return NotFound();

        return Ok(artistReleases);
    }
}
