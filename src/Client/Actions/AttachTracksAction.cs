using BlazorFocused;
using BlazorFocused.Extensions;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Client.Actions;

public class AttachTracksAction : StoreActionAsync<ArtistStore, string>
{
    private readonly ApiOptions apiOptions;
    private readonly IRestClient restClient;
    private readonly ILogger<AttachTracksAction> logger;

    public AttachTracksAction(
        IOptions<ApiOptions> apiOptions,
        IRestClient restClient,
        ILogger<AttachTracksAction> logger)
    {
        this.apiOptions = apiOptions.Value ??
            throw new ArgumentNullException(nameof(apiOptions), "Missing API Options. Check configuration");

        this.restClient = restClient;
        this.logger = logger;
    }

    public override async ValueTask<ArtistStore> ExecuteAsync(string releaseId)
    {
        logger.LogInformation("Get Release {Id} Tracks", releaseId);

        var release = State.Releases.Where(release => release.Id == releaseId).FirstOrDefault();

        if (release is not null && State.ShowPreviewRelease)
        {
            if (!release.Tracks.Any())
            {
                var url = string.Format(apiOptions.ReleaseTracksUrl, releaseId);

                var releaseTracksResult = await restClient.TryGetAsync<IEnumerable<ReleaseTrack>>(url);

                if (releaseTracksResult.IsSuccess)
                {
                    release.Tracks = releaseTracksResult.Value;
                }
                else
                {
                    logger.LogError("Search Artist Error: {Message}", releaseTracksResult.Exception.Message);

                    // TODO: Send exception to top error ribbon or blazor error ui
                    // throw artistSearchResult.Exception;
                }
            }
        }

        return State;
    }
}
