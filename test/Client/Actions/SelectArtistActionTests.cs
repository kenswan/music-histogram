using BlazorMusic.Client.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlazorMusic.Client.Actions;
public class SelectArtistActionTests
{
    private readonly SelectArtistAction selectArtistAction;

    public SelectArtistActionTests()
    {
        selectArtistAction = new SelectArtistAction(NullLogger<SelectArtistAction>.Instance)
        {
            State = new ArtistStore()
        };
    }

    [Fact]
    public void ShouldRetrieveArtistReleasesWhenSelected()
    {
        var artists = TestModels.GenerateArtists();
        var expectedArtist = artists.First();
        var artistId = expectedArtist.Id;

        selectArtistAction.State.Artists = artists;

        var actualState = selectArtistAction.Execute(artistId);

        actualState.CurrentArtist.Should().BeEquivalentTo(expectedArtist);
        actualState.Releases.Should().BeNull();
        actualState.ReleaseError.Should().BeFalse();
    }
}
