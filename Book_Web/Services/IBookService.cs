using Book_MinimalAPI.Models.DTOs;

namespace Book_Web.Services
{
    public interface IBookService
    {
        Task<T> GetAllBooks<T>();
        Task<T> GetBook<T>(int id);
        Task<T> GetBooksFromAuthor<T>(string name);
        Task<T> CreateBook<T>(BookDTO bookDTO);
        Task<T> UpdateBook<T>(BookDTO bookDTO);
        Task<T> DeleteBook<T>(int id);
    }
}
