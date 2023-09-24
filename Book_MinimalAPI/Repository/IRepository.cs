using Book_MinimalAPI.Models;

namespace Book_MinimalAPI.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetSingle(int id);
        Task<T> Create(T toCreate);
        Task<T> Update(int id, T toUpdate);
        Task<T> Delete(int id);
    }
}
