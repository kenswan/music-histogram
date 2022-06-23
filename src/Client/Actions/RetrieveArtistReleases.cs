using BlazorFocused;
using BlazorFocused.Extensions;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Client.Actions;

public class RetrieveArtistReleasesAction : StoreActionAsync<ArtistStore, string>
{
    private readonly ApiOptions apiOptions;
    private readonly IRestClient restClient;
    private readonly ILogger<RetrieveArtistReleasesAction> logger;

    public RetrieveArtistReleasesAction(
        IOptions<ApiOptions> apiOptions,
        IRestClient restClient,
        ILogger<RetrieveArtistReleasesAction> logger)
    {
        this.apiOptions = apiOptions.Value ??
            throw new ArgumentNullException(nameof(apiOptions), "Missing API Options. Check configuration");

        this.restClient = restClient;
        this.logger = logger;
    }

    public override async ValueTask<ArtistStore> ExecuteAsync(string artistId)
    {
        logger.LogInformation("Selected Artist Id: {Input}", artistId);

        var url = (State.ShowPreviewRelease) ?
            string.Format(apiOptions.ArtistMaxReleaseUrl, artistId) :
            string.Format(apiOptions.ArtistReleaseUrl, artistId);

        var releaseResults = await restClient.TryGetAsync<IEnumerable<ArtistRelease>>(url);

        if (releaseResults.IsSuccess)
        {
            State.Releases = releaseResults.Value;
            State.ReleaseError = false;
        }
        else
        {
            logger.LogError("Artist Release Retrieval Error: {Message}", releaseResults.Exception.Message);

            State.ReleaseError = true;
            // TODO: Send exception to top error ribbon or blazor error ui
            // throw releaseResults.Exception;
        }

        return State;
    }
}
