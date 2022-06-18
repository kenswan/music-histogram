using BlazorFocused;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorMusic.Client.Components;

public class ArtistSearchTests
{
    private readonly Mock<IStore<ArtistStore>> artistStoreMock;

    public ArtistSearchTests()
    {
        artistStoreMock = new();
    }

    [Fact]
    public void ShouldRenderAristResults()
    {
        var artists = TestModels.GenerateArtists();
        var expectedArtistNames = artists.Select(artist => artist.Name);
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var store = new ArtistStore { Artists = artists };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(store));

        var component = context.RenderComponent<ArtistSearch>();

        var artistElements = component.FindAll(".artist-search-result");

        var actualArtistNames = artistElements.Select(element => element.TextContent);

        Assert.Equal(artists.Count(), artistElements.Count);

        actualArtistNames.Should().BeEquivalentTo(expectedArtistNames);
    }

    [Fact]
    public void ShouldRenderEmptyWhenNoArtistResults()
    {
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var store = new ArtistStore { Artists = Enumerable.Empty<Artist>() };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(store));

        var component = context.RenderComponent<ArtistSearch>();

        var artistElements = component.FindAll(".artist-search-result");

        artistElements.Should().BeEmpty();
    }
}
