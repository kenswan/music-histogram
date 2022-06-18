using BlazorFocused;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BlazorMusic.Client.Actions;

public class SearchArtistActionTests
{
    private readonly Mock<IRestClient> restClientMock;
    private readonly ApiOptions apiOptions;

    private readonly SearchArtistAction searchArtistAction;

    public SearchArtistActionTests()
    {
        restClientMock = new();

        apiOptions = new ApiOptions
        {
            SearchArtistUrl = TestModels.GenerateRandomRelativeUrl() + "?zero={0}&one={1}"
        };

        searchArtistAction = new SearchArtistAction(
            Options.Create(apiOptions),
            restClientMock.Object,
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

        var apiArtistCollection = new RestClientResponse<ArtistCollection>
        {
            Value = artistCollection
        };

        // TODO: Fix extension method mock
        restClientMock.Setup(client =>
            client.GetAsync<ArtistCollection>(expectedUrl))
                .ReturnsAsync(artistCollection);

        var actualState = await searchArtistAction.ExecuteAsync(searchText);

        // actualState.Artists.Should().BeEquivalentTo(expectedArtists);

        // Assert.Equal(searchText, actualState.CurrentSearchTerm);
    }
}
