using BlazorFocused;
using BlazorFocused.Extensions;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Client.Actions;

public class SelectArtistAction : StoreActionAsync<ArtistStore, string>
{
    private readonly ApiOptions apiOptions;
    private readonly IRestClient restClient;
    private readonly ILogger<SelectArtistAction> logger;

    public SelectArtistAction(
        IOptions<ApiOptions> apiOptions,
        IRestClient restClient,
        ILogger<SelectArtistAction> logger)
    {
        this.apiOptions = apiOptions.Value ??
            throw new ArgumentNullException(nameof(apiOptions), "Missing API Options. Check configuration");

        this.restClient = restClient;
        this.logger = logger;
    }

    public override async ValueTask<ArtistStore> ExecuteAsync(string input)
    {
        logger.LogInformation("Selected Artist Id: {Input}", input);

        State.CurrentArtist = State.Artists.Where(artist => artist.Id == input).FirstOrDefault();

        var url = string.Format(apiOptions.ArtistReleaseUrl, input);

        var releaseResults = await restClient.TryGetAsync<IEnumerable<ArtistRelease>>(url);

        if (releaseResults.IsSuccess)
        {
            State.Releases = releaseResults.Value;
        }
        else
        {
            logger.LogError("Artist Release Retrieval Error: {Message}", releaseResults.Exception.Message);

            // TODO: Send exception to top error ribbon or blazor error ui
            // throw releaseResults.Exception;
        }

        return State;
    }
}
