using BlazorFocused.Extensions;
using BlazorFocused.Tools;
using BlazorMusic.Client.Models;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;

namespace BlazorMusic.Client.Actions;
public class SelectArtistActionTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ApiOptions apiOptions;

    private readonly SelectArtistAction selectArtistAction;

    public SelectArtistActionTests()
    {
        var baseAddress = new Faker().Internet.Url();

        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        apiOptions = new ApiOptions
        {
            ArtistReleaseUrl = TestModels.GenerateRandomRelativeUrl() + "{0}/release"
        };

        selectArtistAction = new SelectArtistAction(
            Options.Create(apiOptions),
            RestClientExtensions.CreateRestClient(simulatedHttp.HttpClient),
            NullLogger<SelectArtistAction>.Instance)
        {
            State = new ArtistStore()
        };
    }

    [Fact]
    public async Task ShouldRetrieveArtistReleasesWhenSelected()
    {
        var artists = TestModels.GenerateArtists();
        var expectedArtist = artists.First();
        var artistId = expectedArtist.Id;
        var releases = TestModels.GenerateArtistReleases();
        var expectedUrl = string.Format(apiOptions.ArtistReleaseUrl, artistId);

        selectArtistAction.State.Artists = artists;

        simulatedHttp
            .SetupGET(expectedUrl)
            .ReturnsAsync(HttpStatusCode.OK, releases);

        var actualState = await selectArtistAction.ExecuteAsync(artistId);

        actualState.CurrentArtist.Should().BeEquivalentTo(expectedArtist);
        actualState.Releases.Should().BeEquivalentTo(releases);
    }
}
