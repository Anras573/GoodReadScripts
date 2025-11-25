using GoodReadScripts.Domain;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Application.Abstractions;

public interface IGoodReadClient
{
    Task<Result<HtmlContent>> GetHtmlContentAsync(GoodReadUrl url, CancellationToken cancellationToken = default);
}