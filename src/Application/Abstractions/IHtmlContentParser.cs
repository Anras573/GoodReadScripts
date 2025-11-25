using GoodReadScripts.Domain;
using GoodReadScripts.Utils;

namespace GoodReadScripts.Application.Abstractions;

public interface IHtmlContentParser
{
    Result<BookMetadata> ParseBookMetadata(HtmlContent htmlContent);
    Result<List<GoodReadUrl>> ParseBookListUrls(HtmlContent htmlContent);
}