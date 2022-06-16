using BlazorMusic.Server.Models;
using BlazorMusic.Server.Providers;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BlazorMusic.Server.Services;

public class ArtistServiceTests
{
    private readonly Mock<IMusicDataProvider> musicDataProviderMock;
    private readonly MusicDataOptions musicDataOptions;

    private readonly IArtistService artistService;

    public ArtistServiceTests()
    {
        musicDataProviderMock = new();
        musicDataOptions = new();

        artistService = new ArtistService(
            musicDataProviderMock.Object,
            Options.Create(musicDataOptions),
            NullLogger<ArtistService>.Instance);
    }
}
