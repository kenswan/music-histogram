using BlazorMusic.Client.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlazorMusic.Client.Actions;
public class TogglePreviewActionTests
{
    private readonly TogglePreviewAction togglePreviewAction;

    public TogglePreviewActionTests()
    {
        togglePreviewAction = new TogglePreviewAction(NullLogger<TogglePreviewAction>.Instance)
        {
            State = new ArtistStore()
        };
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldSendPreviewReleaseAvailability(bool showPreviewRelease)
    {
        var actualState = togglePreviewAction.Execute(showPreviewRelease);

        actualState.ShowPreviewRelease.Should().Be(showPreviewRelease);
    }
}
