using BlazorFocused;
using BlazorMusic.Client.Models;
using BlazorMusic.Client.Provider;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorMusic.Client.Components;
public class ArtistDetailsTests
{
    private readonly Mock<IStore<ArtistStore>> artistStoreMock;
    private readonly Mock<IDateTimeProvider> dateTimeProviderMock;

    public ArtistDetailsTests()
    {
        artistStoreMock = new();
        dateTimeProviderMock = new();
    }

    [Fact]
    public void ShouldRenderUnselectedArtistMessageByDefault()
    {
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var artistStore = new ArtistStore { CurrentArtist = null };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistDetails>();

        var overview = component.FindComponents<ArtistOverview>();
        var releases = component.FindComponents<ArtistReleases>();
        var histogram = component.FindComponents<ArtistHistogram>();

        var unselectedArtistMarkup = @"<h3>ArtistDetails</h3><div class=""unselected-artist"">No artist has been selected</div>";

        Assert.Equal(unselectedArtistMarkup, component.Markup);

        overview.Should().BeEmpty();
        releases.Should().BeEmpty();
        histogram.Should().BeEmpty();
    }

    [Fact]
    public void ShouldRenderAristOverviewByDefault()
    {
        var artist = TestModels.GenerateArtist();
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var artistStore = new ArtistStore { CurrentArtist = artist };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistDetails>();

        var anchorElement = component.Find("#artist-overview");
        anchorElement.Click();

        var overview = component.FindComponents<ArtistOverview>();
        var releases = component.FindComponents<ArtistReleases>();
        var histogram = component.FindComponents<ArtistHistogram>();
        var unselectedArtistPrompt = component.FindAll(".unselected-artist");

        Assert.Single(overview);
        Assert.Empty(releases);
        Assert.Empty(histogram);
        Assert.Empty(unselectedArtistPrompt);
    }

    [Fact]
    public void ShouldRenderAristReleases()
    {
        var artist = TestModels.GenerateArtist();
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        var artistStore = new ArtistStore { CurrentArtist = artist };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistDetails>();

        var anchorElement = component.Find("#artist-releases");
        anchorElement.Click();

        var overview = component.FindComponents<ArtistOverview>();
        var releases = component.FindComponents<ArtistReleases>();
        var histogram = component.FindComponents<ArtistHistogram>();
        var unselectedArtistPrompt = component.FindAll(".unselected-artist");

        Assert.Single(releases);
        Assert.Empty(overview);
        Assert.Empty(histogram);
        Assert.Empty(unselectedArtistPrompt);
    }

    [Fact]
    public void ShouldRenderAristHistogram()
    {
        var artist = TestModels.GenerateArtist();
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);
        context.Services.AddTransient(_ => dateTimeProviderMock.Object);
        var artistStore = new ArtistStore { CurrentArtist = artist };

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(artistStore));

        var component = context.RenderComponent<ArtistDetails>();

        var anchorElement = component.Find("#artist-histogram");
        anchorElement.Click();

        var overview = component.FindComponents<ArtistOverview>();
        var releases = component.FindComponents<ArtistReleases>();
        var histogram = component.FindComponents<ArtistHistogram>();
        var unselectedArtistPrompt = component.FindAll(".unselected-artist");

        Assert.Single(histogram);
        Assert.Empty(overview);
        Assert.Empty(releases);
        Assert.Empty(unselectedArtistPrompt);
    }
}
