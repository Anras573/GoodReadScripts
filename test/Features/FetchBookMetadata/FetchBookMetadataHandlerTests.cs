using GoodReadScripts.Application;
using GoodReadScripts.Features.FetchBookMetadata;
using GoodReadScripts.Test.Fakes;
using GoodReadScripts.Test.TestData;

namespace GoodReadScripts.Test.Features.FetchBookMetadata;

public class FetchBookMetadataHandlerTests
{
    [Fact]
    public async Task HandleAsync_ValidUrl_ReturnsBookMetadata()
    {
        // Arrange
        var url = GoodReadUrlTestData.Create("https://www.goodreads.com/book/show/4671.The_Great_Gatsby");
        var fakeClient = GoodReadClientFake.CreateDefaultBookMetadata();
        var parser = new HtmlContentParser();
        var handler = new FetchBookMetadataHandler(fakeClient, parser);
        var request = new FetchBookMetadataRequest(url);

        // Act
        var response = await handler.HandleAsync(request, CancellationToken.None);

        // Assert
        var expected = DefaultBookMetadata.Create(url);
        response.Switch(actual => Assert.Equivalent(expected, actual.BookMetadata, true), error => Assert.Fail(error.Message));
    }
}