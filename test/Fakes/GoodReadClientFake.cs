using GoodReadScripts.Application.Abstractions;
using GoodReadScripts.Domain;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Test.Fakes;

public class GoodReadClientFake : IGoodReadClient
{
    private readonly string _htmlContent;

    private GoodReadClientFake(string htmlContent)
    {
        _htmlContent = htmlContent;
    }

    public Task<Result<HtmlContent>> GetHtmlContentAsync(GoodReadUrl url, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Result<HtmlContent>>(new HtmlContent(_htmlContent, url));
    }

    public static GoodReadClientFake CreateDefaultBookMetadata()
    {
        var defaultHtmlContent = File.ReadAllText("./Fakes/Data/DefaultBookMetadataResponse.html");
        return new GoodReadClientFake(defaultHtmlContent);
    }
    
    public static GoodReadClientFake CreateDefaultBookListMetadata()
    {
        var defaultHtmlContent = File.ReadAllText("./Fakes/Data/DefaultBookListMetadataResponse.html");
        return new GoodReadClientFake(defaultHtmlContent);
    }
}