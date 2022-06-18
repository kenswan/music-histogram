using BlazorFocused;
using BlazorMusic.Client.Models;
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

    public override ValueTask<ArtistStore> ExecuteAsync(string input)
    {
        logger.LogInformation("Selected Artist Id: {Input}", input);

        State.CurrentArtist = input;
        // TODO: Search for artist info

        return new ValueTask<ArtistStore>(State);
    }
}
