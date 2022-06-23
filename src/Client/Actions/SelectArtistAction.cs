using BlazorFocused;
using BlazorMusic.Client.Models;

namespace BlazorMusic.Client.Actions;

public class SelectArtistAction : StoreAction<ArtistStore, string>
{
    private readonly ILogger<SelectArtistAction> logger;

    public SelectArtistAction(ILogger<SelectArtistAction> logger)
    {
        this.logger = logger;
    }

    public override ArtistStore Execute(string artistId)
    {
        logger.LogInformation("Selected Artist Id: {Input}", artistId);

        State.CurrentArtist = State.Artists.Where(artist => artist.Id == artistId).FirstOrDefault();
        State.Releases = null;
        State.ReleaseError = false;

        return State;
    }
}
