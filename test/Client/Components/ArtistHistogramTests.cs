using BlazorFocused;
using BlazorMusic.Client.Models;
using BlazorMusic.Client.Provider;
using BlazorMusic.Client.Reducers;
using Bogus;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BlazorMusic.Client.Components;
public class ArtistHistogramTests
{
    private readonly Mock<IStore<ArtistStore>> artistStoreMock;
    private readonly Mock<IDateTimeProvider> dateTimeProviderMock;

    public ArtistHistogramTests()
    {
        artistStoreMock = new();
        dateTimeProviderMock = new();
    }

    [Fact]
    public void ShouldRenderHistogramChart()
    {
        using var context = new TestContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        context.Services.AddScoped(_ => artistStoreMock.Object);
        context.Services.AddTransient(_ => dateTimeProviderMock.Object);
        var store = new ArtistStore { CurrentArtist = null };

        var histogramViewModel = new ArtistHistorgramViewModel
        {
            Years = new int[] { 2020, 2021, 2022 },
            Releases = new int[] { 1, 2, 3 },
            CsvFormat = "2020,1\n2021,2\n2022,3",
            ArtistName = new Faker().Person.FullName
        };

        artistStoreMock.Setup(store =>
            store.Reduce<ArtistHistogramReducer, ArtistHistorgramViewModel>(It.IsAny<Action<ArtistHistorgramViewModel>>()))
                .Callback((Action<ArtistHistorgramViewModel> action) => action(histogramViewModel));

        var component = context.RenderComponent<ArtistHistogram>();

        var chart = component.FindComponent<HistogramChart>();

        chart.Should().NotBeNull();
    }

    [Fact]
    public void ShouldDownloadFile()
    {
        using var context = new TestContext();
        context.JSInterop.Mode = JSRuntimeMode.Loose;
        context.Services.AddScoped(_ => artistStoreMock.Object);
        context.Services.AddTransient(_ => dateTimeProviderMock.Object);

        var store = new ArtistStore { CurrentArtist = null };
        var currentDateTime = new DateTime(2022, 06, 20, 12, 34, 56);
        var artistName = new Faker().Person.FullName;
        var expectedFileName = artistName.Replace(" ", "") + "20220620123456";

        var histogramViewModel = new ArtistHistorgramViewModel
        {
            Years = new int[] { 2020, 2021, 2022 },
            Releases = new int[] { 1, 2, 3 },
            CsvFormat = "2020,1\n2021,2\n2022,3",
            ArtistName = artistName
        };

        var plannedInvocation = context.JSInterop.SetupVoid("downloadFileToBrowser", expectedFileName, histogramViewModel.CsvFormat);

        artistStoreMock.Setup(store =>
            store.Reduce<ArtistHistogramReducer, ArtistHistorgramViewModel>(It.IsAny<Action<ArtistHistorgramViewModel>>()))
                .Callback((Action<ArtistHistorgramViewModel> action) => action(histogramViewModel));

        dateTimeProviderMock.Setup(provider => provider.GetDateTimeNow()).Returns(currentDateTime);

        var component = context.RenderComponent<ArtistHistogram>();

        var downloadButton = component.Find(".file-download");

        downloadButton.Click();

        plannedInvocation.VerifyInvoke("downloadFileToBrowser");

        plannedInvocation.SetVoidResult();
    }
}
