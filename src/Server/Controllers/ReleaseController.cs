using BlazorMusic.Server.Services;
using BlazorMusic.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlazorMusic.Server.Controllers;

[Route("api/[controller]")]
[Produces("application/json")]
[ApiController]
public class ReleaseController : ControllerBase
{
    private readonly IReleaseService releaseService;
    private readonly ILogger<ReleaseController> logger;

    public ReleaseController(IReleaseService releaseService, ILogger<ReleaseController> logger)
    {
        this.releaseService = releaseService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all tracks for a given release
    /// </summary>
    /// <param name="id">Unique identifier of release</param>
    /// <returns>List of song tracks</returns>
    [HttpGet("{id}/track")]
    [ProducesResponseType(typeof(IEnumerable<ReleaseTrack>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<ReleaseTrack>>> GetReleaseById([FromRoute] string id)
    {
        logger.LogInformation("Request Details for Release {Id}", id);

        var releaseTracks = await releaseService.RetrieveReleaseTracksByIdAsync(id);

        if (releaseTracks is null || !releaseTracks.Any())
            return NotFound();

        return Ok(releaseTracks);
    }
}
