using Book_MinimalAPI.Models;

namespace Book_MinimalAPI.Data
{
    public class TestStore
    {
        public List<Book> _books = new List<Book>()
        {
            new Book(){Id = 1, Name = "Fishing Guide", Author = "Sal Monella", Available = true, ReleaseDate = new DateTime(2012, 06, 20) },
            new Book(){Id = 2, Name = "How to code in C#", Author = "Cecil Sharp", Available = false, ReleaseDate = new DateTime(2022, 09, 05) },
            new Book(){Id = 3, Name = "Murder in murder lane", Author = "abcffge", Available = true, ReleaseDate = new DateTime(2010, 10, 01) },
            new Book(){Id = 4, Name = "Level 99 woodcutting how to", Author = "Jack Lumber", Available = false, ReleaseDate = new DateTime(1992, 07, 17) },
            new Book(){Id = 5, Name = "Writing dummy data for dummies", Author = "Chet GPT", Available = true, ReleaseDate = new DateTime(2012, 06, 20) }
        };
    }
}
