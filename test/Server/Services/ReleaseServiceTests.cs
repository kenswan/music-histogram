using BlazorMusic.Server.Extensions;
using BlazorMusic.Server.Models;
using BlazorMusic.Server.Providers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BlazorMusic.Server.Services;
public class ReleaseServiceTests
{
    private readonly Mock<IMusicDataProvider> musicDataProviderMock;
    private readonly MusicDataOptions musicDataOptions;

    private readonly IReleaseService releaseService;

    public ReleaseServiceTests()
    {
        musicDataProviderMock = new();
        musicDataOptions = TestModels.GenerateMusicDataOptions();

        releaseService = new ReleaseService(
            musicDataProviderMock.Object,
            Options.Create(musicDataOptions),
            NullLogger<ReleaseService>.Instance);
    }

    [Fact]
    public async Task ShouldRetrieveReleaseTracksByReleaseId()
    {
        var releaseId = TestModels.RandomIdentifier;
        var releaseResponse = TestModels.GenerateReleaseResponse();
        var expectedReleaseTracks = releaseResponse.Media.First().Tracks.ToTracks();

        musicDataProviderMock.Setup(provider =>
            provider.GetReleaseByIdAsync(releaseId))
                .ReturnsAsync(releaseResponse);

        var actualReleaseTracks = await releaseService.RetrieveReleaseTracksByIdAsync(releaseId);

        actualReleaseTracks.Should().BeEquivalentTo(expectedReleaseTracks);
    }
}
