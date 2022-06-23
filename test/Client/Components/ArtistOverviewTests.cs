using BlazorFocused;
using BlazorMusic.Client.Models;
using BlazorMusic.Shared;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorMusic.Client.Components;
public class ArtistOverviewTests
{
    private readonly Mock<IStore<ArtistStore>> artistStoreMock;

    public ArtistOverviewTests()
    {
        artistStoreMock = new();
    }

    [Fact]
    public void ShouldRenderAristInformation()
    {
        var expectedArtist = TestModels.GenerateArtist();
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var artistStore = new ArtistStore { CurrentArtist = expectedArtist };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistOverview>();

        var artistElements = component.FindAll($"#artist-detail-{expectedArtist.Id}");

        Assert.Single(artistElements);
    }

    [Fact]
    public void ShouldRenderEmptyWhenNoArtistResults()
    {
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var artistStore = new ArtistStore { Artists = Enumerable.Empty<Artist>() };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistOverview>();

        var artistElements = component.FindAll($".artist-detail-table");

        artistElements.Should().BeEmpty();
    }
}
