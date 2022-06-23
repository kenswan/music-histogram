using BlazorFocused;
using BlazorMusic.Client.Models;

namespace BlazorMusic.Client.Actions;

public class TogglePreviewAction : StoreAction<ArtistStore, bool>
{
    private readonly ILogger<TogglePreviewAction> logger;

    public TogglePreviewAction(ILogger<TogglePreviewAction> logger)
    {
        this.logger = logger;
    }

    public override ArtistStore Execute(bool showPreviewRelease)
    {
        logger.LogInformation("Preview Mode: {Input}", showPreviewRelease);

        State.ShowPreviewRelease = showPreviewRelease;

        return State;
    }
}
