using BlazorMusic.Server.Services;
using BlazorMusic.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BlazorMusic.Server.Controllers;

[Route("api/[controller]")]
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

    [HttpGet]
    [Produces(typeof(ArtistCollection))]
    public async Task<ActionResult<ArtistCollection>> SearchArtist([FromQuery][Required] string search, [FromQuery] int page = 1)
    {
        logger.LogInformation("Request Artist Search Keyword: {Search}", search);

        var artistCollection = await artistService.SearchArtistsAsync(search, page);

        if (artistCollection is null || !artistCollection.Artists.Any())
            return NotFound();

        return Ok(artistCollection);
    }

    [HttpGet("{id}/release")]
    [Produces(typeof(ArtistCollection))]
    public async Task<ActionResult<IEnumerable<ArtistRelease>>> GetArtistReleases([FromRoute] string id)
    {
        logger.LogInformation("Request Releases for Artist {Id}", id);

        var artistReleases = await artistService.RetrieveAristReleasesAsync(id);

        if (artistReleases is null || !artistReleases.Any())
            return NotFound();

        return Ok(artistReleases);
    }
}
