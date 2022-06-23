using BlazorFocused;
using BlazorMusic.Client.Actions;
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
        var artistStore = new ArtistStore { Artists = artists };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

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
        var artistStore = new ArtistStore { Artists = Enumerable.Empty<Artist>() };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistSearch>();

        var artistElements = component.FindAll(".artist-search-result");

        artistElements.Should().BeEmpty();
    }

    [Fact]
    public void ShouldSelectArtist()
    {
        var artists = TestModels.GenerateArtists();
        var expectedArtist = artists.First();
        var expectedArtistId = expectedArtist.Id;
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var artistStore = new ArtistStore { Artists = artists };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistSearch>();

        var artistAnchor = component.Find($"#artist-select-{expectedArtistId}");

        artistAnchor.Click();

        artistStoreMock.Verify(store => store.Dispatch<SelectArtistAction, string>(expectedArtistId), Times.Once());
        artistStoreMock.Verify(store => store.DispatchAsync<RetrieveReleasesAction, string>(expectedArtistId), Times.Once());
    }
}
