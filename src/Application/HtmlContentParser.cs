using GoodReadScripts.Application.Abstractions;
using GoodReadScripts.Domain;
using GoodReadScripts.Utils;

using HtmlAgilityPack;

namespace GoodReadScripts.Application;

public class HtmlContentParser : IHtmlContentParser
{
    public Result<BookMetadata> ParseBookMetadata(HtmlContent htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent.Content);

        var imageUrl = doc
            .DocumentNode
            .SelectNodes("//img[@class='ResponsiveImage']")
            .FirstOrDefault()?
            .GetAttributeValue("src", string.Empty);

        if (string.IsNullOrWhiteSpace(imageUrl)) return new Error("Image not found");
        if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var imageUri)) return new Error("Invalid image url");

        var title = doc
            .DocumentNode
            .SelectNodes("//h1[@data-testid='bookTitle']")
            .FirstOrDefault()?
            .InnerText
            .Trim();

        if (string.IsNullOrWhiteSpace(title)) return new Error("Title not found");

        var author = doc
            .DocumentNode
            .SelectNodes("//span[@data-testid='name']")
            .FirstOrDefault()?
            .InnerText
            .Trim();

        if (string.IsNullOrWhiteSpace(author)) return new Error("Author not found");

        var pagesFormat = doc
            .DocumentNode
            .SelectNodes("//p[@data-testid='pagesFormat']")
            .FirstOrDefault()?
            .InnerText
            .Split([" "], StringSplitOptions.RemoveEmptyEntries);

        if (pagesFormat == null) return new Error("Pages format not found");

        if (!int.TryParse(pagesFormat[0], out var pages))
            return new Error("Number of pages not found");

        List<string> format = [pagesFormat[^1]];

        var genres = doc
            .DocumentNode
            .SelectNodes("//div[@data-testid='genresList']")
            .FirstOrDefault()?
            .FirstChild
            .ChildNodes
            .Descendants()
            .Where(x => x.Name == "a")
            .Select(a => a.InnerText.Trim())
            .ToList() ?? [];

        if (genres.Count == 0) return new Error("No genres found");

        var metadata = new BookMetadata(
            Author: author,
            Genres: genres,
            Format: format,
            Title: title,
            Image: imageUri,
            NumberOfPages: pages,
            GoodreadUrl: htmlContent.SourceUri,
            HasRead: true);

        return metadata;
    }

    public Result<List<GoodReadUrl>> ParseBookListUrls(HtmlContent htmlContent)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent.Content);
        
        var tableBody = doc
            .DocumentNode
            .SelectSingleNode("//tbody[@id='booksBody']");
        
        var urls = tableBody
            .Descendants("tr")
            .Select(tr => tr
                .SelectSingleNode(".//td[@class='field title']/div/a"))
            .Select(a => a.GetAttributeValue("href", string.Empty))
            .Where(href => !string.IsNullOrWhiteSpace(href))
            .Select(href => GoodReadUrl.Create("https://www.goodreads.com" + href))
            .Where(result => result.IsSuccess)
            .Select(result => result.Match(url => url, _ => null!))
            .ToList();
        
        if (urls.Count == 0)
            return new Error("No book URLs found");
        
        return urls;
    }
}