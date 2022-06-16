using BlazorFocused;
using BlazorMusic.Server.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace BlazorMusic.Server.Providers;

public class MusicDataProviderTests
{
    private readonly Mock<IRestClient> restClientMock;
    private readonly MusicDataOptions musicDataOptions;

    private readonly IMusicDataProvider musicDataProvider;

    public MusicDataProviderTests()
    {
        restClientMock = new();
        musicDataOptions = new();

        musicDataProvider = new MusicDataProvider(
            restClientMock.Object,
            Options.Create(musicDataOptions),
            NullLogger<MusicDataProvider>.Instance);
    }
}
