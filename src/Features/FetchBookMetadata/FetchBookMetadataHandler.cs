using GoodReadScripts.Application.Abstractions;
using GoodReadScripts.Domain;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Features.FetchBookMetadata;

public class FetchBookMetadataHandler(
    IGoodReadClient goodReadClient,
    IHtmlContentParser htmlContentParser)
    : IHandler<FetchBookMetadataRequest, FetchBookMetadataResponse>
{
    public async Task<Result<FetchBookMetadataResponse>> HandleAsync(FetchBookMetadataRequest request, CancellationToken cancellationToken)
    {
        // Pseudo-implementation

        /*
         * 1. Send an HTTP GET request to the Goodreads URL to retrieve the HTML content of the page.
         * 2. Parse the HTML content using an HTML parser library (e.g., HtmlAgilityPack).
         * 3. Extract relevant metadata fields such as:
         *   - Format (e.g., Paperback, eBook)
         *   - Author
         *   - Genres
         *   - Title
         *   - Number of Pages
         *   - Image URL
         *   - Has Read status (if available) // Always assume true!
         *
         * 4. Create and return a BookMetadata object populated with the extracted data.
         */

        var contentResult = await goodReadClient.GetHtmlContentAsync(request.GoodreadUrl, cancellationToken);

        return contentResult.Match<Result<FetchBookMetadataResponse>>(ParseContent, error => error);
    }

    private Result<FetchBookMetadataResponse> ParseContent(HtmlContent content)
    {
        var parseResult = htmlContentParser.ParseBookMetadata(content);

        return parseResult.Match<Result<FetchBookMetadataResponse>>(
            metadata => new FetchBookMetadataResponse(metadata),
            error => error);
    }
}