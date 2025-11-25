using GoodReadScripts.Utils;

namespace GoodReadScripts.Domain;

public record GoodReadUrl(Uri Url)
{
    public static Result<GoodReadUrl> Create(string urlString)
    {
        if (!Uri.TryCreate(urlString, UriKind.Absolute, out var uri))
        {
            return new Error("Invalid URL format.");
        }

        if (uri.Host != "www.goodreads.com" && uri.Host != "goodreads.com")
        {
            return new Error("URL is not a Goodreads URL.");
        }

        return new GoodReadUrl(uri);
    }

    public override string ToString() => Url.ToString();
}