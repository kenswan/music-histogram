using BlazorMusic.Server.Models;

namespace BlazorMusic.Server.Extensions;

public class ArtistSearchResponseExtensionsTests
{
    [Theory]
    [InlineData("2022-02-10", 2022)]
    [InlineData("2022", 2022)]
    [InlineData("02-10-2022", 2022)]
    [InlineData("22", 0)]
    [InlineData("", 0)]
    [InlineData(null, 0)]
    public void ShouldConvertVariousReleaseDateFormats(string inputReleaseDate, int expectedReleaseYear)
    {
        var artistRelease = new ArtistReleaseResponse
        {
            Releases = new List<ReleaseResponse>
            {
                { new ReleaseResponse {
                    Date = inputReleaseDate,
                    Media = TestModels.GenerateMediaResponses(),
                    ReleaseGroup = TestModels.GenerateReleaseGroupResponse(),
                } }
            }
        };

        Assert.Equal(expectedReleaseYear, artistRelease.ToReleases(false).First().Year);
    }
}
