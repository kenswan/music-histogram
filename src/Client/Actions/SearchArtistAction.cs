using BlazorFocused;
using BlazorFocused.Extensions;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Client.Actions;

public class SearchArtistAction : StoreActionAsync<ArtistStore, string>
{
    private readonly ApiOptions apiOptions;
    private readonly IRestClient restClient;
    private readonly ILogger<SearchArtistAction> logger;

    public SearchArtistAction(
        IOptions<ApiOptions> apiOptions,
        IRestClient restClient,
        ILogger<SearchArtistAction> logger)
    {
        this.apiOptions = apiOptions.Value ??
            throw new ArgumentNullException(nameof(apiOptions), "Missing API Options. Check configuration");

        this.restClient = restClient;
        this.logger = logger;
    }

    public override async ValueTask<ArtistStore> ExecuteAsync(string input)
    {
        logger.LogInformation("Searching Text: {Input}", input);

        State.CurrentSearchTerm = input;

        var url = string.Format(apiOptions.SearchArtistUrl, input, 1);

        var artistSearchResult = await restClient.TryGetAsync<ArtistCollection>(url);

        if (artistSearchResult.IsSuccess)
        {
            State.Artists = artistSearchResult.Value.Artists;
        }
        else
        {
            var exception = new ApplicationException("Search Artist Error", artistSearchResult.Exception);
            logger.LogError(exception, "Search Artist Error: {Message}", artistSearchResult.Exception.Message);

            throw exception;
        }

        return State;
    }
}
