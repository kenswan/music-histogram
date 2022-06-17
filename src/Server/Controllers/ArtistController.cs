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

        var artistCollection = await artistService.SearchArtists(search, page);

        if (artistCollection is null || !artistCollection.Artists.Any())
            return NotFound();

        return Ok(artistCollection);
    }
}
