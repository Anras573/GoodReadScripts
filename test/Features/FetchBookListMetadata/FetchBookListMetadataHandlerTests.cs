using GoodReadScripts.Application;
using GoodReadScripts.Features.FetchBookListMetadata;
using GoodReadScripts.Test.Fakes;
using GoodReadScripts.Test.TestData;

namespace GoodReadScripts.Test.Features.FetchBookListMetadata;

public class FetchBookListMetadataHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Successful_Result_When_Client_Returns_Content()
    {
        // Arrange
        var url = GoodReadUrlTestData.Create("https://www.goodreads.com/book/show/4671.The_Great_Gatsby");
        var fakeClient = GoodReadClientFake.CreateDefaultBookListMetadata();
        var parser = new HtmlContentParser();
        var handler = new FetchBookListMetadataHandler(fakeClient, parser);
        var request = new FetchBookListMetadataRequest(url);
        
        // Act
        var response = await handler.HandleAsync(request, CancellationToken.None);
        
        // Assert
        var expected = DefaultBookListMetadata.Create();
        response.Switch(actual => Assert.Equivalent(expected, actual.BookUrls, true), error => Assert.Fail(error.Message));
    }
}