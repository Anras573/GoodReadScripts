using GoodReadScripts.Domain;

namespace GoodReadScripts.Test.TestData;

public static class DefaultBookMetadata
{
    public static BookMetadata Create(GoodReadUrl url)
    {
        return new BookMetadata(
            Format: ["Paperback"],
            GoodreadUrl: url,
            Author: "Masashi Kishimoto",
            Genres: ["Manga", "Fantasy", "Comics", "Fiction", "Shonen", "Graphic Novels", "Action"],
            Title: "Boruto: Naruto Next Generations, Vol. 1: Uzumaki Boruto!!",
            NumberOfPages: 208,
            Image: new Uri("https://m.media-amazon.com/images/S/compressed.photo.goodreads.com/books/1484925211i/32919010.jpg"),
            HasRead: true);
    }
}