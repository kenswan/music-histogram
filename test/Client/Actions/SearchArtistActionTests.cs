using BlazorFocused;
using BlazorFocused.Tools;
using BlazorMusic.Client.Models;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;
using Xunit.Abstractions;

namespace BlazorMusic.Client.Actions;

public class SearchArtistActionTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly IServiceCollection serviceCollection;
    private readonly ApiOptions apiOptions;

    private readonly SearchArtistAction searchArtistAction;

    public SearchArtistActionTests(ITestOutputHelper testOutputHelper)
    {
        var baseAddress = new Faker().Internet.Url();

        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        serviceCollection = new ServiceCollection();

        serviceCollection
            .AddSingleton<IConfiguration>(new ConfigurationBuilder().Build())
            .AddRestClient(baseAddress)
            .AddSimulatedHttp(simulatedHttp);

        // BlazorFocused Extension method "TryGet" is being used and won't allow mocking
        // To work-around, using a real instance and passing in a mock httpclient provided by BlazorFocused Tools
        using var serviceProvider = serviceCollection.BuildServiceProvider();
        var restClient = serviceProvider.GetRequiredService<IRestClient>();

        apiOptions = new ApiOptions
        {
            SearchArtistUrl = TestModels.GenerateRandomRelativeUrl() + "?zero={0}&one={1}"
        };

        searchArtistAction = new SearchArtistAction(
            Options.Create(apiOptions),
            restClient,
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
