using BlazorFocused;
using BlazorMusic.Client.Models;
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
        var store = new ArtistStore { Releases = releases };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(store));

        var component = context.RenderComponent<ArtistReleases>();

        var releaseElements = component.FindAll(".accordion-header");

        Assert.Equal(releases.Count(), releaseElements.Count);
    }

    [Fact]
    public void ShouldRenderEmptyWhenNoReleasesFound()
    {
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var store = new ArtistStore { Releases = Enumerable.Empty<ArtistRelease>() };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(store));

        var component = context.RenderComponent<ArtistReleases>();

        var artistElements = component.FindAll(".accordion-header");

        artistElements.Should().BeEmpty();
    }
}
