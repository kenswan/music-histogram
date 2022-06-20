using BlazorFocused.Extensions;
using BlazorFocused.Tools;
using BlazorMusic.Client.Models;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;

namespace BlazorMusic.Client.Actions;

public class SearchArtistActionTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ApiOptions apiOptions;

    private readonly SearchArtistAction searchArtistAction;

    public SearchArtistActionTests()
    {
        var baseAddress = new Faker().Internet.Url();

        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        apiOptions = new ApiOptions
        {
            SearchArtistUrl = TestModels.GenerateRandomRelativeUrl() + "?zero={0}&one={1}"
        };

        searchArtistAction = new SearchArtistAction(
            Options.Create(apiOptions),
            RestClientExtensions.CreateRestClient(simulatedHttp.HttpClient),
            NullLogger<SearchArtistAction>.Instance)
        {
            State = new ArtistStore()
        };
    }

    [Fact]
    public async Task ShouldSearchArtistUsingInputText()
    {
        var searchText = TestModels.RandomString;
        var artistCollection = TestModels.GenerateArtistCollection();
        var expectedArtists = artistCollection.Artists;
        var expectedUrl = string.Format(apiOptions.SearchArtistUrl, searchText, 1);

        simulatedHttp
            .SetupGET(expectedUrl)
            .ReturnsAsync(HttpStatusCode.OK, artistCollection);

        var actualState = await searchArtistAction.ExecuteAsync(searchText);

        actualState.Artists.Should().BeEquivalentTo(expectedArtists);

        Assert.Equal(searchText, actualState.CurrentSearchTerm);
    }
}
