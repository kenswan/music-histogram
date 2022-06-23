using BlazorFocused;
using BlazorMusic.Client.Actions;
using BlazorMusic.Client.Models;
using BlazorMusic.Client.Reducers;
using BlazorMusic.Shared;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorMusic.Client.Components;
public class ArtistReleasesTests
{
    private readonly Mock<IStore<ArtistStore>> artistStoreMock;

    public ArtistReleasesTests()
    {
        artistStoreMock = new();
    }

    [Fact]
    public void ShouldRenderAristResults()
    {
        var releases = TestModels.GenerateArtistReleases();
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var releaseViewModel = new ArtistReleasesViewModel { FilteredReleases = releases };

        artistStoreMock.Setup(store =>
            store.Reduce<ArtistReleasesReducer, ArtistReleasesViewModel>(It.IsAny<Action<ArtistReleasesViewModel>>()))
                .Callback((Action<ArtistReleasesViewModel> action) => action(releaseViewModel));

        var component = context.RenderComponent<ArtistReleases>();

        var releaseElements = component.FindAll(".accordion-header");

        Assert.Equal(releases.Count(), releaseElements.Count);
    }

    [Fact]
    public void ShouldRenderEmptyWhenNoReleasesFound()
    {
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var releaseViewModel = new ArtistReleasesViewModel { FilteredReleases = Enumerable.Empty<ArtistRelease>() };

        artistStoreMock.Setup(store =>
            store.Reduce<ArtistReleasesReducer, ArtistReleasesViewModel>(It.IsAny<Action<ArtistReleasesViewModel>>()))
                .Callback((Action<ArtistReleasesViewModel> action) => action(releaseViewModel));

        var component = context.RenderComponent<ArtistReleases>();

        var artistElements = component.FindAll(".accordion-header");

        artistElements.Should().BeEmpty();
    }

    [Fact]
    public void ShouldSelectReleaseToViewTracks()
    {
        var releases = TestModels.GenerateArtistReleases();
        var expectedRelease = releases.Last();
        var expectedReleaseId = expectedRelease.Id;

        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var releaseViewModel = new ArtistReleasesViewModel { FilteredReleases = releases };

        artistStoreMock.Setup(store =>
            store.Reduce<ArtistReleasesReducer, ArtistReleasesViewModel>(It.IsAny<Action<ArtistReleasesViewModel>>()))
                .Callback((Action<ArtistReleasesViewModel> action) => action(releaseViewModel));

        var component = context.RenderComponent<ArtistReleases>();

        var releaseButton = component.Find($"#select-release-{expectedReleaseId}");

        releaseButton.Click();

        artistStoreMock.Verify(store => store.DispatchAsync<AttachTracksAction, string>(expectedReleaseId), Times.Once());
    }
}
