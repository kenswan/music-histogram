using BlazorFocused.Extensions;
using BlazorFocused.Tools;
using BlazorMusic.Client.Models;
using Bogus;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BlazorMusic.Client.Actions;

public class RetrieveReleasesActionTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ApiOptions apiOptions;

    private readonly RetrieveReleasesAction retrieveReleasesAction;

    public RetrieveReleasesActionTests()
    {
        var baseAddress = new Faker().Internet.Url();

        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        apiOptions = new ApiOptions
        {
            ArtistReleaseUrl = TestModels.GenerateRandomRelativeUrl() + "{0}/release?allReleases=true",
            ArtistMaxReleaseUrl = TestModels.GenerateRandomRelativeUrl() + "{0}/release?allReleases=false"
        };

        retrieveReleasesAction = new RetrieveReleasesAction(
            Options.Create(apiOptions),
            RestClientExtensions.CreateRestClient(simulatedHttp.HttpClient),
            NullLogger<RetrieveReleasesAction>.Instance)
        {
            State = new ArtistStore()
        };
    }
}
