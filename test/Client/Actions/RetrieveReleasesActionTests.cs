using BlazorFocused.Extensions;
using BlazorFocused.Tools;
using BlazorMusic.Client.Models;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;

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
            ArtistMaxReleaseUrl = TestModels.GenerateRandomRelativeUrl() + "{0}/release?allReleases=false",
        };

        retrieveReleasesAction = new RetrieveReleasesAction(
            Options.Create(apiOptions),
            RestClientExtensions.CreateRestClient(simulatedHttp.HttpClient),
            NullLogger<RetrieveReleasesAction>.Instance)
        {
            State = new ArtistStore()
        };
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldGetStandardReleases(bool hasError)
    {
        var artistId = TestModels.RandomIdentifier;
        var expectedReleases = !hasError ? TestModels.GenerateArtistReleases() : null;
        var statusCode = !hasError ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        retrieveReleasesAction.State.ShowPreviewRelease = false;

        var expectedUrl = string.Format(apiOptions.ArtistReleaseUrl, artistId);

        simulatedHttp
            .SetupGET(expectedUrl)
            .ReturnsAsync(statusCode, expectedReleases);

        var actualState = await retrieveReleasesAction.ExecuteAsync(artistId);

        if (!hasError)
        {
            actualState.Releases.Should().BeEquivalentTo(expectedReleases);
            actualState.ReleaseError.Should().BeFalse();
        }
        else
        {
            actualState.Releases.Should().BeNull();
            actualState.ReleaseError.Should().BeTrue();
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldGetMaxReleases(bool hasError)
    {
        var artistId = TestModels.RandomIdentifier;
        var expectedReleases = !hasError ? TestModels.GenerateArtistReleases() : null;
        var statusCode = !hasError ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
        retrieveReleasesAction.State.ShowPreviewRelease = true;

        var expectedUrl = string.Format(apiOptions.ArtistMaxReleaseUrl, artistId);

        simulatedHttp
            .SetupGET(expectedUrl)
            .ReturnsAsync(statusCode, expectedReleases);

        var actualState = await retrieveReleasesAction.ExecuteAsync(artistId);

        if (!hasError)
        {
            actualState.Releases.Should().BeEquivalentTo(expectedReleases);
            actualState.ReleaseError.Should().BeFalse();
        }
        else
        {
            actualState.Releases.Should().BeNull();
            actualState.ReleaseError.Should().BeTrue();
        }
    }
}
