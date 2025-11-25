namespace GoodReadScripts.Domain;

// Template for book metadata
/*
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
title: Jujutsu Kaisen vol 1
Number of Pages: 192
Goodread URL: https://www.goodreads.com/book/show/42075170-1-jujutsu-kaisen-1
Image: https://i.gr-assets.com/images/S/compressed.photo.goodreads.com/books/1537971660l/42075170.jpg
Has Read: true
---
*/

public record BookMetadata(
    List<string> Format,
    string Author,
    List<string> Genres,
    string Title,
    int NumberOfPages,
    GoodReadUrl GoodreadUrl,
    Uri Image,
    bool HasRead
)
{
    public string ToMarkdown()
    {
        var formatLines = string.Join(Environment.NewLine, Format.Select(f => $"  - {f}"));
        var genreLines = string.Join(Environment.NewLine, Genres.Select(g => $"  - {g}"));

        return
            $"""
             ---
             Format:
             {formatLines}
             Author: {Author}
             Genres:
             {genreLines}
             title: "{Title}"
             Number of Pages: {NumberOfPages}
             Goodread URL: {GoodreadUrl}
             Image: {Image}
             Has Read: {HasRead.ToString().ToLower()}
             ---
             """;
    }
}