using BlazorFocused.Extensions;
using BlazorFocused.Tools;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;

namespace BlazorMusic.Client.Actions;

public class AttachTracksActionTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ApiOptions apiOptions;

    private readonly AttachTracksAction attachTracksAction;

    public AttachTracksActionTests()
    {
        var baseAddress = new Faker().Internet.Url();

        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        apiOptions = new ApiOptions
        {
            ReleaseTracksUrl = TestModels.GenerateRandomRelativeUrl() + "release/{0}/track"
        };

        attachTracksAction = new AttachTracksAction(
            Options.Create(apiOptions),
            RestClientExtensions.CreateRestClient(simulatedHttp.HttpClient),
            NullLogger<AttachTracksAction>.Instance)
        {
            State = new ArtistStore()
        };
    }

    [Fact]
    public async Task ShouldGetReleaseTracks()
    {
        var releaseId = TestModels.RandomIdentifier;
        var selectedRelease = new ArtistRelease { Id = releaseId, Tracks = Enumerable.Empty<ReleaseTrack>() };

        var releases = TestModels.GenerateArtistReleases()
            .Concat(new List<ArtistRelease> { selectedRelease });

        var expectedTracks = TestModels.GenerateReleaseTracks();
        var expectedUrl = string.Format(apiOptions.ReleaseTracksUrl, releaseId);

        attachTracksAction.State.ShowPreviewRelease = true;
        attachTracksAction.State.Releases = releases;

        simulatedHttp
            .SetupGET(expectedUrl)
            .ReturnsAsync(HttpStatusCode.OK, expectedTracks);

        var actualState = await attachTracksAction.ExecuteAsync(releaseId);
        var actualRelease = actualState.Releases.Where(release => release.Id == releaseId).First();

        actualRelease.Tracks.Should().BeEquivalentTo(expectedTracks);
    }

    [Fact]
    public async Task ShouldThrowOnSReleaseTrackRetrievalError()
    {
        var releaseId = TestModels.RandomIdentifier;
        var selectedRelease = new ArtistRelease { Id = releaseId, Tracks = Enumerable.Empty<ReleaseTrack>() };

        var releases = TestModels.GenerateArtistReleases()
            .Concat(new List<ArtistRelease> { selectedRelease });

        var expectedUrl = string.Format(apiOptions.ReleaseTracksUrl, releaseId);
        attachTracksAction.State.ShowPreviewRelease = true;
        attachTracksAction.State.Releases = releases;

        simulatedHttp
            .SetupGET(expectedUrl)
            .ReturnsAsync(HttpStatusCode.BadGateway, null);

        var exception = await Assert.ThrowsAsync<ApplicationException>(() =>
            attachTracksAction.ExecuteAsync(releaseId).AsTask());

        Assert.Contains($"Release {releaseId} Track Retrieval Error", exception.Message);
    }
}
