using Book_MinimalAPI.Data;
using Book_MinimalAPI.Models;
using Book_MinimalAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Book_MinimalAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext db;

        public BookRepository(AppDbContext dbInject)
        {
            db = dbInject;
        }

        public async Task<Book> Create(Book toCreate)
        {
            if (toCreate != null)
            {
                await db.Books.AddAsync(toCreate);
                await db.SaveChangesAsync();
            }
            return toCreate;
        }

        public async Task<Book> Delete(int id)
        {
            Book bookToDelete = await db.Books.FindAsync(id);
            if (bookToDelete != null)
            {
                db.Books.Remove(bookToDelete);
                await db.SaveChangesAsync();
            }
            return bookToDelete;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await db.Books.ToListAsync();
        }

        public async Task<Book> GetSingle(int id)
        {
            Book result = await db.Books.FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<Book>> GetFromAuthor(string authorName)
        {
            var result = await db.Books.Where(b => b.Author.ToLower() == authorName.ToLower()).ToListAsync();
            return result;
        }

        public async Task<Book> Update(int id, Book toUpdate)
        {
            Book bookToEdit = await db.Books.FindAsync(id);
            if (bookToEdit != null)
            {
                bookToEdit.Name = toUpdate.Name;
                bookToEdit.Author = toUpdate.Author;
                bookToEdit.Available = toUpdate.Available;
                bookToEdit.ReleaseDate = toUpdate.ReleaseDate;
                await db.SaveChangesAsync();
            }
            return bookToEdit;

        }
    }
}
