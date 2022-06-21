using BlazorFocused;
using BlazorMusic.Client.Models;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Client.Actions;

public class SelectArtistAction : StoreAction<ArtistStore, string>
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

    public override ArtistStore Execute(string artistId)
    {
        logger.LogInformation("Selected Artist Id: {Input}", artistId);

        State.CurrentArtist = State.Artists.Where(artist => artist.Id == artistId).FirstOrDefault();
        State.Releases = null;
        State.ReleaseError = false;

        return State;
    }
}
