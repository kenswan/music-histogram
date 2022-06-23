using BlazorFocused;
using BlazorMusic.Client.Actions;
using BlazorMusic.Client.Models;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorMusic.Client.Components;

public class TogglePreviewTests
{
    private readonly Mock<IStore<ArtistStore>> artistStoreMock;

    public TogglePreviewTests()
    {
        artistStoreMock = new();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldRenderTogglePreviewIfEnabled(bool enabled)
    {
        var toggleDivId = "#advanced-release-toggle";
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);

        context.Services.AddOptions<ApiOptions>()
            .Configure(options => options.MaxReleaseEnabled = enabled);

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(new ArtistStore()));

        var component = context.RenderComponent<TogglePreview>();

        if (enabled)
            Assert.NotNull(component.Find(toggleDivId));
        else
            Assert.Throws<ElementNotFoundException>(() => component.Find(toggleDivId));
    }

    [Fact]
    public void ShouldEnablePreview()
    {
        var toggleInputId = "#togglePreview";
        using var context = new TestContext();
        context.Services.AddScoped(_ => artistStoreMock.Object);

        context.Services.AddOptions<ApiOptions>()
            .Configure(options => options.MaxReleaseEnabled = true);

        artistStoreMock.Setup(store => store.Subscribe(It.IsAny<Action<ArtistStore>>()))
            .Callback((Action<ArtistStore> action) => action(new ArtistStore()));

        var component = context.RenderComponent<TogglePreview>();

        var toggleInput = component.Find(toggleInputId);

        toggleInput.Click();

        artistStoreMock.Verify(store => store.Dispatch<TogglePreviewAction, bool>(true), Times.Once());

        toggleInput.Click();

        artistStoreMock.Verify(store => store.Dispatch<TogglePreviewAction, bool>(false), Times.Once());
    }
}
