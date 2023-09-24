using Book_MinimalAPI.Models;

namespace Book_MinimalAPI.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetFromAuthor(string authorName);
    }
}
