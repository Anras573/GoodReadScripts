using GoodReadScripts.Application.Abstractions;
using GoodReadScripts.Domain;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Infrastructure;

public class GoodReadClient(HttpClient client) : IGoodReadClient
{
    public async Task<Result<HtmlContent>> GetHtmlContentAsync(GoodReadUrl url, CancellationToken cancellationToken)
    {
        try
        {
            var response = await client.GetStringAsync(url.Url, cancellationToken);
            if (string.IsNullOrEmpty(response)) return new Error("No content found");

            return new HtmlContent(response, url);
        }
        catch (Exception ex)
        {
            return new Error(ex.Message);
        }
    }
}