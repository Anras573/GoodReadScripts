using GoodReadScripts.Domain;

namespace GoodReadScripts.Test.Domain;

public class BookMetadataTests
{
    [Fact]
    public void CanConvertToMarkdown()
    {
        // Arrange
        var goodreadUrlResult = GoodReadUrl.Create("https://www.goodreads.com/book/show/42075170-1-jujutsu-kaisen-1");
        Assert.True(goodreadUrlResult.IsSuccess, "Failed to create GoodReadUrl.");
        var goodreadUrl = goodreadUrlResult.Match(url => url, error => throw new Exception(error.Message));

        var metadata = new BookMetadata(
            Format: ["Manga", "Paperback"],
            Author: "Gege Akutami",
            Genres: ["Horror", "Fantasy", "Graphic Novels", "Supernatural", "Fiction", "Shonen"],
            Title: "Jujutsu Kaisen vol 1",
            NumberOfPages: 192,
            GoodreadUrl: goodreadUrl,
            Image: new Uri("https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1537971660l/42075170.jpg"),
            HasRead: true);

        // Act
        var markdown = metadata.ToMarkdown();

        // Assert
        const string expected =
            """
            ---
            Format:
              - Manga
              - Paperback
            Author: Gege Akutami
            Genres:
              - Horror
              - Fantasy
              - Graphic Novels
              - Supernatural
              - Fiction
              - Shonen
            title: "Jujutsu Kaisen vol 1"
            Number of Pages: 192
            Goodread URL: https://www.goodreads.com/book/show/42075170-1-jujutsu-kaisen-1
            Image: https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1537971660l/42075170.jpg
            Has Read: true
            ---
            """;

        Assert.Equal(expected, markdown);
    }
}