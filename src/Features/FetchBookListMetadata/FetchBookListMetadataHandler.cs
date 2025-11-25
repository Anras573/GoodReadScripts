using GoodReadScripts.Application.Abstractions;
using GoodReadScripts.Domain;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Features.FetchBookListMetadata;

public class FetchBookListMetadataHandler(IGoodReadClient client, IHtmlContentParser parser)
    : IHandler<FetchBookListMetadataRequest, FetchBookListMetadataResponse>
{
    public async Task<Result<FetchBookListMetadataResponse>> HandleAsync(FetchBookListMetadataRequest request, CancellationToken cancellationToken)
    {
        var contentResult = await client.GetHtmlContentAsync(request.GoodReadUrl, cancellationToken);

        return contentResult.Match(ParseContent, error => error);
    }

    private Result<FetchBookListMetadataResponse> ParseContent(HtmlContent content)
    {
        var parseResult = parser.ParseBookListUrls(content);
        
        return parseResult.Match<Result<FetchBookListMetadataResponse>>(
            urls => new FetchBookListMetadataResponse(urls),
            error => error);
    }
}